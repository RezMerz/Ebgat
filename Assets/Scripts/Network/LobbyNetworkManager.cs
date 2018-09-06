using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyNetworkManager : NetworkManager {

    public GameObject lobbyClientPrefab;
    public GameObject lobbyManagerPrefab;

    List<ClientData> clientsData;
    private int id;
    private LobbyManager lobbyManager;
    private bool isServer = false;
    private bool[] isSlotFull;


    public void Start()
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

    }

    private void Update()
    {
        
    }

    public override void OnServerConnect(NetworkConnection conn)
    {
        base.OnServerConnect(conn);
        Debug.Log(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        base.OnClientConnect(conn);
        Debug.Log(conn);
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject lobbyClientObj = Instantiate(lobbyClientPrefab);
        LobbyClient lobbyClient = lobbyClientObj.GetComponent<LobbyClient>();
        lobbyClient.id = ++id;
        int slot = 0;
        for (int i = 0; i < isSlotFull.Length; i++){
            if(!isSlotFull[i]){
                isSlotFull[i] = true;
                slot = i + 1;
                break;
            }
        }
        lobbyClient.slot = slot;
        clientsData.Add(new ClientData(id, slot));
        NetworkServer.AddPlayerForConnection(conn, lobbyClientObj, playerControllerId);
    }

    public override void OnStartHost()
    {
        base.OnStartHost();
        isServer = true;
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
            if (slot > 3)
                team = 2;
            else
                team = 1;
        }
    }
}

