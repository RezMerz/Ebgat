using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkSender : NetworkBehaviour
{

    PlayerControl playerControl;
    CharacterAttributes charStats;
    ServerNetwork serverNetwork;

    private string data = "";

    private static int num = 1;
    [SyncVar]public int PlayerID;



    // Use this for initialization
    void Start()
    {
        if (isServer)
        {
            PlayerID = num++;
        }
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        serverNetwork = playerControl.serverNetwork;
        if (playerControl.IsLocalPlayer())
        {
            charStats.teamName = "Team 1";
            charStats.enemyTeamName = "Team 2";
            gameObject.layer = LayerMask.NameToLayer("Team 1");

            //change color for localm player
            playerControl.color = Color.green;
            //GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            
            charStats.teamName = "Team 2";
            charStats.enemyTeamName = "Team 1";
            gameObject.layer = LayerMask.NameToLayer("Team 2");

            playerControl.color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerControl.IsLocalPlayer())
            return;
        if (data.Equals(""))
            return;
        SendCommands();
    }

    private void SendCommands()
    {
        serverNetwork.CmdRecievecommands(data, PlayerID);
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
    public void MoveVertical(int j)
    {

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

    public void RangedAttack(Vector2 attackDir){
        data += 8 + "," + attackDir.x + "," + attackDir.y + ",\n";
    }

    public void MeleeAttack(Vector2 attackDir)
    {
        data += 9 + "," + attackDir.x + "," + attackDir.y + ",\n";
    }


}
