using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {
    
    public GameObject serverNetwork;
    public GameObject clientNetworkReciever;
    public GameObject serverManager;
    public List<GameObject> serverSidePlayers;
    public List<GameObject> clientSidePlayers;
    public List<Transform> heroSpawnPositions;
    public GameObject playerConnection;

    private bool flag;
    bool start;

    public int playerNumber { get; set; }

    private int playerID;
    public Hashtable connectionTable;
    public List<PlayerConnection> playerConnections;

    public int maxPlayerCount;

    private void Start()
    {
        connectionTable = new Hashtable();
        playerConnections = new List<PlayerConnection>();
        playerID = 0;
        start = true;
        flag = true;
    }

    private void Update()
    {
        if(NetworkServer.active && flag){
            flag = false;
            GameObject server = Instantiate(serverNetwork);
            GameObject clientNetwork = Instantiate(clientNetworkReciever);
            GameObject srvmanager = Instantiate(serverManager);
            NetworkServer.Spawn(clientNetwork);
            NetworkServer.Spawn(server);
            NetworkServer.Spawn(srvmanager);
        } 
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        connectionTable.Add(++playerID, conn);
        GameObject playercon = Instantiate(playerConnection);
        PlayerConnection p = playercon.GetComponent<PlayerConnection>();
        p.clientId = playerID;
        playerConnections.Add(p);
        NetworkServer.AddPlayerForConnection(conn, playercon, playerControllerId);

    }

    /*public override void OnClientDisconnect(NetworkConnection conn)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }*/

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        StopHost();
        GetComponent<CustomNetworkDiscovery>().Initialize();
        GetComponent<CustomNetworkDiscovery>().StopBroadcast();
    }

    public override void OnStopHost()
    {
        base.OnStopHost();
        GetComponent<CustomNetworkDiscovery>().Initialize();
        GetComponent<CustomNetworkDiscovery>().StopBroadcast();
        Start();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void StartHost(int maxPlayerCount)
    {
        this.maxPlayerCount = maxPlayerCount;
        base.StartHost();
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        if(ServerManager.instance.currentClientCount < ServerManager.instance.maxClientCount)
            base.OnClientConnect(conn);
    }
}