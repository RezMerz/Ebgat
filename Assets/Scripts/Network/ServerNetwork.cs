using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Globalization;

public class ServerNetwork : NetworkBehaviour
{

    public PlayerControl playerControl;// { get; set;}
    string data = "";

    public void SetPlayerControl(PlayerControl playerControl){
        this.playerControl = playerControl;
    }

    [Command]
    public void CmdRecievecommands(string commands, int playerID)
    {
        string[] lines = commands.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] parts = lines[i].Split(',');
            switch (parts[0])
            {
                case "1": playerControl.MoveRight(); break;
                case "2": playerControl.MoveLeft(); break;
                case "3": playerControl.MoveFinished(); break;
                case "4": playerControl.JumpPressed(); break;
                case "6": playerControl.JumpReleased(); break;
             //   case "7": playerControl.SetVerticalDirection(Convert.ToInt32(parts[1])); break;
                case "8": playerControl.AttackPressed(); break;
                case "9": playerControl.AttackReleased(); break;
                case "10": playerControl.Ability1Pressed(); break;
                case "11": playerControl.Ability1Hold(); break;
                case "12": playerControl.Ability1Released(); break;
                case "13": playerControl.Ability2Pressed(); break;
                case "14": playerControl.Ability2Hold(); break;
                case "15": playerControl.Ability2Released(); break;
                case "16": playerControl.DropDownPressed(); break;
                case "17": playerControl.DropDownReleased(); break;
                case "18": playerControl.DashPressed(); break;
                case "19": playerControl.deltaYAim(float.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat)); break;
                case "20": playerControl.deltaXAim(float.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat)); break;
                case "21": playerControl.AimPressed(); break;
                case "22": playerControl.AimReleased(); break;
                case "23": playerControl.AimController(new Vector2(float.Parse(parts[1]),float.Parse(parts[2]))); break;
            }
        }
    }

    [Command]
    public void CmdKillPlayer()
    {
        Destroy(gameObject);
    }

    [Command]
    public void CmdSendWorldStateToClient(int requesterId, int frameId){
        ServerManager.instance.SendWorldStateToClient(requesterId, frameId);
    }


    [Command]
    public void CmdClientConnected(int clientId){
        StartCoroutine(ss(clientId));
    }

    IEnumerator ss(int clientId){
        yield return new WaitForSeconds(1);
        ServerManager.instance.ClientConnected(clientId);
    }
}
