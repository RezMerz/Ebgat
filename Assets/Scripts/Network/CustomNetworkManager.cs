using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    /// <summary>
    /// offline
    /// </summary>


    public GameObject lobbyClientPrefab;
    public GameObject lobbyManagerPrefab;

    List<ClientData> clientsData;
    private int id;
    private LobbyManager lobbyManager;
    private bool isServer = false;
    private bool[] isSlotFull;


    /// <summary>
    /// online
    /// </summary>


    public GameObject serverNetwork;
    public GameObject clientNetworkReciever;
    public GameObject serverManager;
    public List<GameObject> serverSidePlayers;
    public List<GameObject> clientSidePlayers;
    public List<Transform> heroSpawnPositions;
    public GameObject playerConnectionPrefab;

    private bool flag;

    public int playerNumber { get; set; }

    private int playerID;
    public Hashtable connectionTable;
    public List<PlayerConnection> playerConnections;

    public int maxPlayerCount;
    public float baseRespawnTime;
    public float respawnTimePenalty;
    public bool isInfinite;

    private NetworkConnection networkConnection;

    private void Start()
    {
        id = 0;
        clientsData = new List<ClientData>();
        isSlotFull = new bool[6];
        if (NetworkServer.active && isServer)
        {
            GameObject lobbyManagerObj = Instantiate(lobbyManagerPrefab);
            lobbyManager = lobbyManagerObj.GetComponent<LobbyManager>();
            NetworkServer.Spawn(lobbyManagerObj);
        }

        connectionTable = new Hashtable();
        playerConnections = new List<PlayerConnection>();
        playerID = 0;
        flag = true;
    }

    private void Update()
    {
        if(NetworkServer.active && flag && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == onlineScene){
            flag = false;
            InstantiateGameSceneObjects();
        } 
    }

    public void InstantiateGameSceneObjects(){
        GameObject server = Instantiate(serverNetwork);
        GameObject clientNetwork = Instantiate(clientNetworkReciever);
        GameObject srvmanager = Instantiate(serverManager);
        NetworkServer.Spawn(clientNetwork);
        NetworkServer.Spawn(server);
        NetworkServer.Spawn(srvmanager);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject lobbyClientObj = Instantiate(lobbyClientPrefab);
        LobbyClient lobbyClient = lobbyClientObj.GetComponent<LobbyClient>();
        lobbyClient.id = ++id;
        int slot = 0;
        for (int i = 0; i < isSlotFull.Length; i++)
        {
            if (!isSlotFull[i])
            {
                isSlotFull[i] = true;
                slot = i + 1;
                break;
            }
        }
        lobbyClient.slot = slot;
        clientsData.Add(new ClientData(id, slot));
        NetworkServer.AddPlayerForConnection(conn, lobbyClientObj, playerControllerId);

        /*Debug.Log(conn.connectionId);
        connectionTable.Add(++playerID, conn);
        GameObject playercon = Instantiate(playerConnectionPrefab);
        PlayerConnection p = playercon.GetComponent<PlayerConnection>();
        p.clientId = playerID;
        playerConnections.Add(p);
        NetworkServer.AddPlayerForConnection(conn, playercon, playerControllerId);*/

    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        networkConnection = conn;
        //RegisterNetworkClient();
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
        
        var s = new System.Diagnostics.StackTrace();
        Debug.Log(s.GetFrame(4).GetMethod().Name);
        Debug.Log("Stoping host");
        base.OnStopHost();
        GetComponent<CustomNetworkDiscovery>().Initialize();
        GetComponent<CustomNetworkDiscovery>().StopBroadcast();
        Start();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        isServer = true;
    }

    public void StartHost(int maxPlayerCount, float baseRespawnTime, float respawnTimePenalty, bool isInfinite)
    {
        this.maxPlayerCount = maxPlayerCount;
        base.StartHost();
        this.baseRespawnTime = baseRespawnTime;
        this.respawnTimePenalty = respawnTimePenalty;
        this.isInfinite = isInfinite;
    }

    public void StartGame()
    {
        if (isServer)
        {

            GameObject.FindWithTag("NetworkDiscovery").GetComponent<NetworkDiscovery>().StopBroadcast();
            ServerChangeScene(onlineScene);
            //Destroy(GetComponent<NetworkDiscovery>());
            //StartCoroutine(SendRpc(0));
        }
    }

    IEnumerator SendRpc(int time)
    {
        yield return new WaitForSeconds(time);
        lobbyManager.RpcStartGame();
    }

    /*public override void OnClientConnect(NetworkConnection conn)
    {
        if(ServerManager.instance.currentClientCount < ServerManager.instance.maxClientCount)
            base.OnClientConnect(conn);
    }*/

    private void RegisterNetworkClient(){
        Debug.Log("registered");
        Debug.Log(networkConnection);
        Debug.Log(networkConnection.connectionId);
        NetworkClient networkClient = new NetworkClient(networkConnection);
        networkClient.RegisterHandler(MsgType.Highest + 1, GeAbsoluteState);
    }

    void GeAbsoluteState(NetworkMessage netMsg)
    {
        Debug.Log("Client connected");
    }

    public void SetClientDataOnServer(int id, string name)
    {
        Debug.Log(id + "   " + name);
        for (int i = 0; i < clientsData.Count; i++)
        {
            if (clientsData[i].id == id)
            {
                clientsData[i].name = name;
            }
        }
    }

    public string GetLobbyData()
    {
        string output = "";
        for (int i = 0; i < clientsData.Count; i++)
        {
            output += clientsData[i].slot + "&" + clientsData[i].name + "$";
        }
        return output;
    }



    public void ChangeTeam(int id)
    {
        for (int i = 0; i < clientsData.Count; i++)
        {
            if (clientsData[i].id == id)
            {
                if (clientsData[i].slot < 4)
                {
                    for (int j = 3; j < isSlotFull.Length; j++)
                    {
                        if (!isSlotFull[j])
                        {
                            isSlotFull[j] = true;
                            isSlotFull[clientsData[i].slot - 1] = false;
                            clientsData[i].slot = j + 1;
                            clientsData[i].UpdateTeam();
                            return;
                        }
                    }
                }
                else
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (!isSlotFull[j])
                        {
                            isSlotFull[j] = true;
                            isSlotFull[clientsData[i].slot - 1] = false;
                            clientsData[i].slot = j + 1;
                            clientsData[i].UpdateTeam();
                            return;
                        }
                    }
                }
            }

        }
    }

    class ClientData
    {
        public string name;
        public int id;
        public int slot;
        public int team;

        public ClientData(int id, int slot)
        {
            this.id = id;
            this.slot = slot;
        }

        public void UpdateTeam(){
            if (slot > 3)
                team = 2;
            else
                team = 1;
        }
    }
}