using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkSender : NetworkBehaviour {

    public static ServerNetworkSender instance;


    ClientNetworkReciever clientNetworkReciever;
    string data = "";
    string hitData = "";
    string[] worldData;
    int attackID;

    public int networkSendTime = 3;
    private string[] worldStates;
    private int currentTime = 0;
    private int currentid;

    private void Awake()
    {
        instance = this;
        worldStates = new string[networkSendTime];
    }

    void Start()
    {
        clientNetworkReciever = ClientNetworkReciever.instance;
    }
	
    // Update is called once per frame
    void Update()
    {
    }

    public void RegisterWorldState(WorldState worldState){
        string s = worldState.GetWorldData();
        if (s.Length == 0)
            return;
        worldStates[currentTime] = s;
        currentTime++;
        if(currentTime == networkSendTime){
            clientNetworkReciever.RpcRecieveWorldData(worldStates, ServerManager.instance.CurrentStateID);
            ServerManager.instance.CurrentStateID++;
            currentTime = 0;
            worldStates = new string[networkSendTime];
        }

    }

    public void SendWorldFullstate(WorldState worldState, int requesterID){
        CustomNetworkManager networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        for (int i = 0; i < networkManager.playerConnections.Count; i++)
        {
            if (networkManager.playerConnections[i].clientId == requesterID)
            {
                Debug.Log(ServerManager.instance.CurrentStateID + currentTime);
                networkManager.playerConnections[i].SendAbsoluteState(new AbsoluteStateMessage(worldState.GetWorldData(), ServerManager.instance.CurrentStateID + currentTime));
                return;
            }
        }
        //int id = ServerManager.instance.CurrentStateID * 3 + currentTime;
        //clientNetworkReciever.RpcRecieveWorldstate(worldState.GetWorldData(), id, requesterID);
    }
}
