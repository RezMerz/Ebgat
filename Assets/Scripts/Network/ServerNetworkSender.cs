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

    private void SendCommands()
    {
        clientNetworkReciever.RpcRecieveCommands(data, hitData);
        data = "";
        hitData = "";
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
        int id = ServerManager.instance.CurrentStateID++ * 3 + currentTime;
        clientNetworkReciever.RpcRecieveWorldstate(worldState.GetWorldData(), id, requesterID);
    }
}
