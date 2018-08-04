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
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        serverNetwork = playerControl.serverNetwork;
        if (playerControl.IsLocalPlayer())
        {
            /*charStats.teamName = "Team 1";
            charStats.enemyTeamName = "Team 2";
            gameObject.layer = LayerMask.NameToLayer("Team 1");
            */
            //change color for localm player
            playerControl.color = Color.green;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else
        {
            /*
            charStats.teamName = "Team 2";
            charStats.enemyTeamName = "Team 1";
            gameObject.layer = LayerMask.NameToLayer("Team 2");
            */
            playerControl.color = Color.white;
        }

        if(isServer){
            PlayerID = num++;
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

    public void KillPlayer()
    {
        data += ECommand.KillPlayer.ToString() + ",\n";
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

    public void SetVerticalDirection(int num)
    {
        data += 7 + "," + num + ",\n";
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

    public void Shootbullet(Vector3 targetdirection, Vector3 origin, float bulletDamage)
    {
        data += ECommand.ShootBullet.ToString() + "," + targetdirection.x + "," + targetdirection.y + "," + targetdirection.z + "," + origin.x + "," + origin.y + "," + origin.z + "," + bulletDamage + ",\n";
    }

    public void TakeDamage(float damage)
    {
        data += ECommand.TakeAttack.ToString() + "," + damage + ",\n";
    }

    public void MeleeAttack(Vector3 origin, float damage, Vector3 targetdirection, int layer)
    {
        data += ECommand.MeleeAttack.ToString() + "," + origin.x + "," + origin.y + "," + origin.z + "," + damage + "," + targetdirection.x + "," + targetdirection.y + "," + targetdirection.z + layer + ",\n";
    }
}

public enum ECommand
{
    Move, MoveFinished, JumpPressed, JumpLong, JumpReleased, ShootBullet, KillPlayer, TakeAttack, MeleeAttack
}
