using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyNetworkManager : NetworkManager {
    
    public GameObject lobbyClientPrefab;
    public GameObject lobbyManagerPrefab;

    List<ClientData> clientsData;
    private int id;
    private int slot;
    private LobbyManager lobbyManager;

    public void Start()
    {
        id = 0;
        slot = 0;
        clientsData = new List<ClientData>();

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
        lobbyClient.slot = ++slot;
        clientsData.Add(new ClientData(id, slot));
        NetworkServer.AddPlayerForConnection(conn, lobbyClientObj, playerControllerId);
    }

    public override void OnStartHost()
    {
        GameObject lobbyManagerObj = Instantiate(lobbyManagerPrefab);
        NetworkServer.Spawn(lobbyManagerObj);
    }

    public void SetClientDataOnServer(int id, string name){
        for (int i = 0; i < clientsData.Count; i++){
            if(clientsData[i].id == id){
                clientsData[i].name = name;
            }
        }
    }

    public string GetLobbyData(){
        string output = "";
        for (int i = 0; i < clientsData.Count; i++){
            output += clientsData[i].slot + "&" + clientsData[i].name + "$";
        }
        return output;
    }

    class ClientData
    {
        public string name;
        public int id;
        public int slot;

        public ClientData(int id, int slot){
            this.id = id;
            this.slot = slot;
        }
    }
}

