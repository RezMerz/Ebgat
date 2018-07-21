using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Globalization;

public class ClientNetworkReciever : NetworkBehaviour {

    PlayerControl playerControl;
    CharacterMove characterMove;
    PlayerJump jump;

    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
    }

    [ClientRpc]
    public void RpcRecieveCommands(string data){
        string[] lines = data.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++){
            string[] parts = lines[i].Split(',');
            /*Debug.Log(parts.Length);
            for (int j = 0; j < parts.Length; j++){
                Debug.Log(parts[j]);
            }*/
            ECommand cmd = (ECommand)Enum.Parse(typeof(ECommand), parts[0]);
            switch(cmd){
                case ECommand.Move: RpcMove(Convert.ToInt32(parts[1])); break;
                case ECommand.MoveFinished: RpcMoveFinished(new Vector3(float.Parse(parts[1]),float.Parse(parts[2]), float.Parse(parts[3]))); break;
                case ECommand.JumpPressed: RpcJumpPressed(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]))); break;
                case ECommand.JumpHold: RpcJumpHold(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]))); break;
                case ECommand.JumpReleased: RpcJumpReleased(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]))); break;
                case ECommand.ShootBullet: RpcShootBullet(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])), new Vector3(float.Parse(parts[4]), float.Parse(parts[5]), float.Parse(parts[6])), float.Parse(parts[7])); break;
                case ECommand.TakeAttack: RpcTakeAttack(float.Parse(parts[0])); break;
            }
        }
    }

    public void RpcMove(int num)
    {
        if (isLocalPlayer)
        {
            return;
        }
        characterMove.MovePressed(num);
    }

    public void RpcMoveFinished(Vector3 position)
    {
        if (isLocalPlayer)
        {
            return;
        }
        Debug.Log("RPC MoveFINISHED");
        transform.position = position;
    }

    public void RpcJumpPressed(Vector3 position)
    {
        if (isLocalPlayer)
            return;
        transform.position = position;
        jump.JumpPressed();
    }

    public void RpcJumpHold(Vector3 position)
    {
        if (isLocalPlayer)
            return;
        transform.position = position;
        jump.JumpHold();
    }

    public void RpcJumpReleased(Vector3 position)
    {
        if (isLocalPlayer)
            return;
        transform.position = position;
        jump.JumpReleased();
    }

    public void RpcShootBullet(Vector3 targetdirection, Vector3 origin, float bulletDamage)
    {
        
    }

    public void RpcTakeAttack(float damage)
    {
        if (isServer)
            return;
        playerControl.TakeAttack(damage, null);
    }
}
