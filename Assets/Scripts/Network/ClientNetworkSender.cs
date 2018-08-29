using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkSender : NetworkBehaviour
{

    ServerNetwork serverNetworkReciever;
    PlayerConnection playerConnection;

    private string data = "";

    private static int num = 1;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start()
    {
        playerConnection = GetComponent<PlayerConnection>();
        playerConnection.SetClientNetworkSender(this);
        serverNetworkReciever = GetComponent<ServerNetwork>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerConnection.isLocalPlayer)
            return;
        if (data.Equals(""))
            return;
        SendCommands();
    }

    public void RequestWorldState(int playerId){
        serverNetworkReciever.CmdSendWorldStateToClient(playerId);
    }

    private void SendCommands()
    {
        serverNetworkReciever.CmdRecievecommands(data, playerConnection.clientId);
        data = "";
    }


    public void Move(int num)
    {
        if (num == 1)
            data += 1 + ",\n";
        else if (num == -1)
            data += 2 + ",\n";
        else
            Debug.Log("wrong input");
    }

    public void MoveFinished(Vector3 position)
    {
        data += 3 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void JumpPressed(Vector3 position)
    {
        data += 4 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void JumpLong(Vector3 position)
    {
        data += 5 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void JumpReleased(Vector3 position)
    {
        data += 6 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void SetVerticalDirection(int num)
    {
        data += 7 + "," + num + ",\n";
    }

    public void AttackPressed(){
        data += 8 + ",\n";
    }

    public void AttackReleased()
    {
        data += 9 + ",\n";
    }

    public void Ability1Pressed()
    {
        data += 10 + ",\n";
    }

    public void Ability1Hold()
    {
        data += 11 + ",\n";
    }
    public void Ability1Released()
    {
        data += 12 + ",\n";
    }
    public void Ability2Pressed()
    {
        data += 13 + ",\n";
    }

    public void Ability2Hold()
    {
        data += 14 + ",\n";
    }

    public void Ability2Released()
    {
        data += 15 + ",\n";
    }

    public void DropDownPressed()
    {
        data += 16 + ",\n";
    }
    public void DropDownReleased()
    {
        data += 17 + ",\n";
    }

    public void DashPressed()
    {
        data += 18 + ",\n";
    }
}
