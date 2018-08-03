using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.Networking.Match;
using UnityEngine.Networking.Types;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages all different kinds of client server connection from match making to direct connection
/// </summary>
public class MatchManager : NetworkMatch
{
	public static MatchManager instance;

	/// <summary>
	/// The client's player Id in the game
	/// </summary>
	public int playerID;

	/// <summary>
	/// Should the client connect to a server set in IP and port
	/// If false will use discovery on the LAN
	/// </summary>
	public bool directConnection = false;

	private bool isStartedDiscovery;

	/// <summary>
	/// Is this client the local client on the server
	/// </summary>
	private bool isLocalClient;

	/// <summary>
	/// IP address of the server for local connection
	/// </summary>
	public string serverIP = "127.0.0.1";

	/// <summary>
	/// The port that the server listens too, used both for client and server
	/// </summary>
	public int serverPort = 5000;

	/// <summary>
	/// The network client that we use
	/// </summary>
	public NetworkClient nc;

	/// <summary>
	/// List of matches returned from Unity match maker
	/// </summary>
	private List<MatchInfoSnapshot> matches;

	/// <summary>
	/// The discovery manager component which allows us to find servers on the LAN
	/// </summary>
	private LANDiscoveryManager discoveryService;

	private void Start()
	{
		instance = this;
		discoveryService = GetComponent<LANDiscoveryManager>();
		if (Application.platform != RuntimePlatform.WebGLPlayer)
		{
			discoveryService.Initialize();
		}
	}

	/// <summary>
	/// Connects either to the IP and port specified in the inspector
	/// or use LAN discovery based on the directConnection checkbox
	/// </summary>
	public void ConnectLocally()
	{
		if (!directConnection)
		{
			//If not direct then wait for a message from a server on the LAN
			discoveryService.OnServerFound += (address, data) =>
			{
				//When received we should check if we already created the client or not
				//if yes just do nothing
				if (nc != null)
					return;
				//Otherwise set ip and port of the server from the received data
				//create the config, set handlers and connect
				this.serverIP = address;

				ConnectToDedicatedServerAndRegisterHandlers();
			};
			if (!isStartedDiscovery)
			{
				discoveryService.StartAsClient();
				isStartedDiscovery = true;
			}
		}
		else
		{
			//If using direct connection simply create a config
			//set handlers and connect
			ConnectToDedicatedServerAndRegisterHandlers();
		}

	}

	/// <summary>
	/// Connects to server based on the already set IP and port and 
	/// registers handlers for all messages
	/// The ip can be set from any type of match maker, LAN discovery or just in the editor/UI
	/// This method does not care about the way you obtain the IP and port
	/// </summary>
	private void ConnectToDedicatedServerAndRegisterHandlers()
	{
		ConnectionConfig cc = CreateConnectionConfig();
		nc = new NetworkClient();
		nc.Configure(cc, 1);
		RegisterHandlers();
		nc.Connect(serverIP, serverPort);
	}

	/// <summary>
	/// Registers all handlers
	/// </summary>
	private void RegisterHandlers()
	{
		nc.RegisterHandler(CustomNetworkMessages.SetPlayerIdOnClient, netMsg =>
		{
			playerID = netMsg.ReadMessage<IntegerMessage>().value;
			ClientScene.AddPlayer(netMsg.conn, 0);
		});
		nc.RegisterHandler(MsgType.Connect, OnConnectedToGameServer);
		nc.RegisterHandler(MsgType.Disconnect, OnDisConnectedFromServer);
	}

	/// <summary>
	/// Starts a dedicated server without a client in it
	/// </summary>
	public void CreateDedicatedServer()
	{
		Server.instance.StartServer(true);
		discoveryService.StartAsServer();
		isStartedDiscovery = true;
	}

	/// <summary>
	/// This is called when you successfully connect to the server
	/// </summary>
	/// <param name="netMsg"></param>
	private void OnConnectedToGameServer(NetworkMessage netMsg)
	{
		if (isStartedDiscovery && !isLocalClient)
		{
			discoveryService.StopBroadcast();
			isStartedDiscovery = false;
		}
		print("Connected to server");
	}

	/// <summary>
	/// This is called both at disconnection time and when you fail to 
	/// connect to the server
	/// </summary>
	/// <param name="netMsg"></param>
	private void OnDisConnectedFromServer(NetworkMessage netMsg)
	{
		if (isStartedDiscovery)
			discoveryService.StopBroadcast();
		isStartedDiscovery = false;
		print("Disconnected from server");
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		nc.handlers.Clear();
	}

	/// <summary>
	/// Creates a match in Unity MatchMaker and joins it as the host
	/// (both server and client)
	/// </summary>
	public void CreateMatch()
	{
		base.CreateMatch("friendly match", 4, true, "", "", "", 0, 0, (success, info, resp) =>
			  {
				  if (success)
				  {

					  if (Server.instance.StartServer(false, resp))
					  {
						  //To connect to the server in the current unity instance
						  //(i.e host mode) use ClientScene
						  //The network client is returned to you as well and you don't need to create it
						  Debug.Log("start local client");
						  nc = ClientScene.ConnectLocalServer();
						  isLocalClient = true;
						  RegisterHandlers();
					  }
				  }
			  });
	}

	/// <summary>
	/// Tries to list all available matches in unity match maker
	/// and joins the first one
	/// </summary>
	public void ListMatches()
	{
		base.ListMatches(0, 20, "", false, 0, 0, (success, info, resp) =>
			  {
				  if (success)
				  {
					  matches = resp;
					  Debug.Log("match count is " + matches.Count);
					  JoinMatch(resp[matches.Count - 1].networkId);
				  }
				  else
				  {
					  matches = new List<MatchInfoSnapshot>();
				  }
			  });
	}

	/// <summary>
	/// Tries to join a match in unity match maker
	/// </summary>
	/// <param name="ID"></param>
	public void JoinMatch(NetworkID ID)
	{
		base.JoinMatch(ID, "", "", "", 0, 0, (success, info, Response) =>
			   {
				   if (success)
				   {
					   ConnectionConfig cc = CreateConnectionConfig();
					   nc = new NetworkClient();
					   nc.Configure(cc, 1);
					   RegisterHandlers();

					   Utility.SetAccessTokenForNetwork(Response.networkId,
						   new NetworkAccessToken(Response.accessToken.ToString()));
					   nc.Connect(Response);
				   }
				   else
				   {
					   Debug.LogError(Response.ToString());
				   }
			   });
	}

	/// <summary>
	/// Creates a connection config for the game
	/// this should be the same on the server and client
	/// </summary>
	/// <returns></returns>
	public static ConnectionConfig CreateConnectionConfig()
	{
		ConnectionConfig cc = new ConnectionConfig();
		//Add the channels we need
		cc.AddChannel(QosType.ReliableSequenced);
		cc.AddChannel(QosType.Unreliable);
		//If 95% of packets fail then we lower the speed of sends
		cc.NetworkDropThreshold = 95;
		cc.SendDelay = 0;
		return cc;
	}

	/// <summary>
	/// Returns the network delay based on a timestamp sent from the other side of 
	/// the network. You can get a timestamp to send by using NetworkTransport.GetNetworkTimestamp()
	/// </summary>
	/// <param name="stamp"></param>
	/// <returns></returns>
	public float GetTimeDelayOnClient(int stamp)
	{
		byte err;
		return NetworkTransport.GetRemoteDelayTimeMS(nc.connection.hostId, nc.connection.connectionId, stamp, out err) / 1000f;
	}

	public void RegisterHandler(short msgType, NetworkMessageDelegate handler)
	{
		if (nc != null)
		{
			nc.RegisterHandlerSafe(msgType, handler);
		}
	}
}