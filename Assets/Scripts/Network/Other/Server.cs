using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Linq;
using Random = UnityEngine.Random;

public class Server : MonoBehaviour
{
	public static Server instance;

	/// <summary>
	/// If true then will use websocket for webGL games
	/// </summary>
	public bool useWebSockets;

	/// <summary>
	/// We instantiate this prefab for player on the server
	/// </summary>
	public GameObject playerPrefab;

	/// <summary>
	/// The port that the server listens to
	/// </summary>
	public int port = 5000;

	/// <summary>
	/// Maximum number of players the server accepts
	/// Keep in mind that this should be at max equal to player place holders in a map
	/// </summary>
	public int maxPlayers = 4;

	/// <summary>
	/// Name of the game (used for MatchMaker
	/// </summary>
	public string gameName = "best";

	/// <summary>
	/// Game type used for the MatchMaker
	/// </summary>
	public string gameType = "NoOp Fight";

	/// <summary>
	/// We put all positions which players can be created in this list
	/// </summary>
	private List<Vector3> creationPositions = new List<Vector3>();

	public Transform[] positionObjects;

	/// <summary>
	/// A map of Network connection to player IDs
	/// </summary>
	private Dictionary<NetworkConnection, int> connectionToPlayerIDMap;

	/// <summary>
	/// A map of Network connection to player positions
	/// </summary>
	private Dictionary<NetworkConnection, Vector3> connectionToPlayerPositionMap;

	/// <summary>
	/// Another map from connection to player tanks
	/// </summary>
	private Dictionary<NetworkConnection, GameObject> connectionToPlayerObjectMap;

	public int GetPlayerCount()
	{
		return connectionToPlayerObjectMap.Count;
	}

	void Awake()
	{
		//Just set the singleton 
		if (instance != null)
		{
			Debug.LogError("Only one instance of Server should exist");
			DestroyImmediate(this.gameObject);
			return;
		}
		instance = this;
		connectionToPlayerObjectMap = new Dictionary<NetworkConnection, GameObject>();
		connectionToPlayerPositionMap = new Dictionary<NetworkConnection, Vector3>();
		connectionToPlayerIDMap = new Dictionary<NetworkConnection, int>();
		for (int i = 0; i < positionObjects.Length; ++i)
		{
			AddPosition(positionObjects[i].position);
		}
		Application.runInBackground = true;
	}

	private void OnDestroy()
	{
		NetworkServer.Shutdown();
		connectionToPlayerIDMap = null;
		connectionToPlayerPositionMap = null;
		connectionToPlayerObjectMap = null;
	}

	/// <summary>
	/// Returns the network connection for a player id
	/// </summary>
	/// <param name="player"></param>
	/// <returns></returns>
	public NetworkConnection GetConnectionForPlayer(int player)
	{
		return connectionToPlayerIDMap.First(p => p.Value == player).Key;
	}

	/// <summary>
	/// Returns the player Id for a connection
	/// </summary>
	/// <param name="con"></param>
	/// <returns></returns>
	public int GetPlayerForConnection(NetworkConnection con)
	{
		return connectionToPlayerIDMap[con];
	}

	/// <summary>
	/// Gets the delay between a connection and the server using a timestamp sent from
	/// the client
	/// </summary>
	/// <param name="con">Client's connection</param>
	/// <param name="timestamp">The timestamp calculated and sent from the client using NetworkTransport.GetNetworkTimestamp</param>
	/// <returns></returns>
	public float GetDelay(NetworkConnection con, int timestamp)
	{
		byte err;
		return NetworkTransport.GetRemoteDelayTimeMS(NetworkServer.serverHostId, con.connectionId, timestamp, out err) / 1000f;
	}


	/// <summary>
	/// Starts a server, It can be dedicated or not and on the match maker or just
	/// on the system's network interfaces based on parameters
	/// </summary>
	/// <param name="isDedicated"></param>
	/// <param name="matchInfo"></param>
	/// <returns></returns>
	public bool StartServer(bool isDedicated, UnityEngine.Networking.Match.MatchInfo matchInfo = null)
	{
		//Configure the game server
		//For more info see MatchManager.CreateConnectionConfig
		ConnectionConfig cc = MatchManager.CreateConnectionConfig();
		NetworkServer.useWebSockets = useWebSockets;
		NetworkServer.Configure(cc, maxPlayers);

		//Register handler callbacks for messages we can receive

		//We receive this when a client connects
		NetworkServer.RegisterHandler(MsgType.Connect, x =>
		{
			Debug.Log("player added " + x.conn.connectionId);
			var playerId = IDAllocator.GetNext();
			connectionToPlayerIDMap.Add(x.conn, playerId);
			NetworkServer.SendToClient(x.conn.connectionId, CustomNetworkMessages.SetPlayerIdOnClient, new IntegerMessage(playerId));
		});
		//Receive this when a client disconnects due to error/timeout or by requesting a disconnect
		NetworkServer.RegisterHandler(MsgType.Disconnect, _OnPlayerDisconnected);

		//When clients make themselves ready, Unity sends this message to the server
		//Some games use this message to create the player object
		NetworkServer.RegisterHandler(MsgType.AddPlayer, OnPlayerConnectedToGameServer);

		//Try to create the server
		//If isDedicated is true then we create a server listenning to the
		//local network interface, otherwise we should have MatchInfo and listen using Unity's MatchMaker
		bool isSuccessful;
		if (isDedicated)
		{
			isSuccessful = NetworkServer.Listen(port);
		}
		else
		{
			isSuccessful = NetworkServer.Listen(matchInfo, port);
		}

		//If succeeded, initialize network objects in the scene
		if (isSuccessful)
		{
			//Create/enable network objects in the scene
			//If you don't call SpawnObjects then the NetworkObjects in the scene (i.e. objects with NetworkIdentity component)
			//will remain inactive and won't be synced to clients either
			Debug.Log("spawned scene objects with NetworkIdentity");
			NetworkServer.SpawnObjects();
		}
		return isSuccessful;
	}

	/// <summary>
	/// Called when a player is connected
	/// </summary>
	/// <param name="netMsg"></param>
	private void OnPlayerConnectedToGameServer(NetworkMessage netMsg)
	{
		InitializePlayerDataAndAddPlayer(netMsg);
	}

	/// <summary>
	/// Generates an Id for the player and chooses position for him/her
	/// then instantiates the player
	/// </summary>
	/// <param name="netMsg"></param>
	/// <returns></returns>
	private void InitializePlayerDataAndAddPlayer(NetworkMessage netMsg)
	{
		Debug.Log("player with ID " + connectionToPlayerIDMap[netMsg.conn] + " is ready");




		var positionIndex = Random.Range(0, creationPositions.Count);
		var pos = creationPositions[positionIndex];
		creationPositions.RemoveAt(positionIndex);
		connectionToPlayerPositionMap[netMsg.conn] = pos;

		InstantiatePlayer(connectionToPlayerIDMap[netMsg.conn], netMsg.conn, pos);
	}

	/// <summary>
	/// Creates the player tank and initializes the syncvars
	/// which should be sent to the client
	/// </summary>
	/// <param name="player"></param>
	/// <param name="con"></param>
	/// <returns></returns>
	public GameObject InstantiatePlayer(int player, NetworkConnection con, Vector3 position)
	{
		//Instantiate player object
		GameObject go = Instantiate(playerPrefab,
				  position, Quaternion.identity) as GameObject;

		//This method will send the spawn message to the clients and the object will be 
		//recreated there with all syncvars set, it will be owned by the player specified and
		//that player can send commands to the server for the object
		NetworkServer.AddPlayerForConnection(con, go, 0);

		connectionToPlayerObjectMap[con] = go;
		//##PLAYER_JOINED
		return go;
	}

	/// <summary>
	/// Called when a player gets disconnected, removes the player from the game
	/// </summary>
	/// <param name="netMsg"></param>
	private void _OnPlayerDisconnected(NetworkMessage netMsg)
	{
		//let's destroy the player and all of his/her objects.
		NetworkServer.DestroyPlayersForConnection(netMsg.conn);

		//If possible return the color and position of the player to their lists and then clean all maps and data structures
		if (connectionToPlayerIDMap.ContainsKey(netMsg.conn))
		{
			int playerID = connectionToPlayerIDMap[netMsg.conn];
			AddPosition(connectionToPlayerPositionMap[netMsg.conn]);
			NetworkServer.Destroy(connectionToPlayerObjectMap[netMsg.conn]);
			connectionToPlayerIDMap.Remove(netMsg.conn);
			connectionToPlayerPositionMap.Remove(netMsg.conn);
			connectionToPlayerObjectMap.Remove(netMsg.conn);
		}
	}

	/// <summary>
	/// Adds a position to creation positions
	/// </summary>
	/// <param name="position"></param>
	public void AddPosition(Vector3 position)
	{
		creationPositions.Add(position);
	}

	/// <summary>
	/// Gets the position of one of the creation positions
	/// </summary>
	/// <param name="index"></param>
	/// <returns></returns>
	public Vector3 GetPosition(int index)
	{
		return creationPositions[index];
	}

	/// <summary>
	/// Registers a message handler on the server
	/// </summary>
	/// <param name="msgType"></param>
	/// <param name="handler"></param>
	public void RegisterHandler(short msgType,NetworkMessageDelegate handler)
	{
		NetworkServer.RegisterHandler(msgType, handler);
	}
}

/// <summary>
/// A very simplistic ID allocator which always increments its value and returns it. Can be used for systems which need to allocate a limited number of IDs
/// fast and the number of requests in the runtime of the application is less than 2^31
/// </summary>
public static class IDAllocator
{
	private static int lastId = 0;

	public static int GetNext()
	{
		//the condition should almost never be true, fact is if the ids return to 0 then those around 0 might be still used but we are not making ids for a long running
		//process so the id should start from 1 and goes to at most 20-30
		//otherwise another allocator which keeps track of free Ids should be used
		if (lastId >= int.MaxValue)
			lastId = 0;
		return ++lastId;
	}
}