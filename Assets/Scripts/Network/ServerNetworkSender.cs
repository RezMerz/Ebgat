using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetworkSender : NetworkBehaviour {

    public static ServerNetworkSender instance;


    ClientNetworkReciever clientNetworkReciever;
    string data = "";
    string hitData = "";
    int attackID;

    private void Awake()
    {
        instance = this;

    }

    void Start()
    {
        clientNetworkReciever = ClientNetworkReciever.instance;
    }
	
    // Update is called once per frame
    void Update()
    {
        if (!isServer)
            return;
        if (data.Equals(""))
            return;
        SendCommands();
    }

    private void SendCommands()
    {
        clientNetworkReciever.RpcRecieveCommands(data, hitData);
        data = "";
        hitData = "";
    }

    public void ClientMove(int playerID, Vector3 position)
    {
        data += playerID + "," + 1 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void ClientMoveFinished(int playerID, Vector3 position)
    {
        data += playerID + "," + 2 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void ClientSetVerticalSide(int playerID, int num)
    {
        data += playerID + "," + 6 + "," + num + ",\n";
    }

    public void ClientRangedAttack(int playerID, Vector2 attackDir){
        data += playerID + "," + 7 + "," + attackDir.x + "," + attackDir.y + "," + (attackID++) + ",\n";
    }

    public void ClientBulletHit(int attackID){
        data += 8 + "," + attackID + ",\n";
    }
}
