using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Networking;

public class ClientNetworkSender : NetworkBehaviour {

    PlayerControl playerControl;
    CharacterAttributes charStats;
    ServerNetwork serverNetwork;

    private string data = "";

    // Use this for initialization
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        serverNetwork = playerControl.serverNetwork;
        if (isLocalPlayer)
        {
            charStats.teamName = "Team 1";
            charStats.enemyTeamName = "Team 2";
            gameObject.layer = LayerMask.NameToLayer("Team 1");

            //change color for localm player
            playerControl.color = Color.green;
            GetComponent<SpriteRenderer>().color = Color.green;
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
        if (!isLocalPlayer)
            return;
        if (data.Equals(""))
            return;
        SendCommands();
        //data = "";
    }

    private void SendCommands(){
        serverNetwork.CmdRecievecommands(data);
        data = "";
    }

    public void KillPlayer()
    {
        data += ECommand.KillPlayer.ToString() + ",\n"; 
    }

    public void Move(int num)
    {
        data += ECommand.Move.ToString() + "," + num + ",\n";
    }

    public void MoveFinished(Vector3 position)
    {
        data += ECommand.MoveFinished.ToString() + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void JumpPressed(Vector3 position)
    {
        data += ECommand.JumpPressed.ToString() + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void JumpHold(Vector3 position)
    {
        data += ECommand.JumpHold.ToString() + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void JumpReleased(Vector3 position)
    {
        data += ECommand.JumpReleased.ToString() + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void Shootbullet(Vector3 targetdirection, Vector3 origin, float bulletDamage)
    {
        data += ECommand.ShootBullet.ToString() + "," + targetdirection.x + "," + targetdirection.y + "," + targetdirection.z + "," + origin.x + "," + origin.y + "," + origin.z + "," + bulletDamage + ",\n";
    }

    public void TakeDamage(float damage){
        data += ECommand.TakeAttack.ToString() + "," + damage + ",\n";
    }
}

public enum ECommand
{
    Move, MoveFinished, JumpPressed, JumpHold, JumpReleased, ShootBullet, KillPlayer, TakeAttack
}
