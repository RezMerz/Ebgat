using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour
{
    private CharacterAttributes charStats;
    public Moveable characterMove { get; private set; }
    public PlayerJump jump { get; private set; }
    public Attack attack { get; private set; }

    // Use this for initialization
    void Awake()
    {
        charStats = GetComponent<CharacterAttributes>();
        characterMove = GetComponent<Moveable>();
        jump = GetComponent<PlayerJump>();
        attack = GetComponent<Attack>();
        Camera.main.GetComponent<Camera_Follow>().player_ = gameObject;
    }
    ///////////  NETWORK //////////

    [Command]
    public void CmdMove(int num)
    {
        RpcMove(num);
    }

    [Command]
    public void CmdMoveFinished(Vector3 position)
    {
        RpcMoveFinished(position);
    }

    [Command]
    public void CmdJump(Vector3 position)
    {
        RpcJump(position);
    }

    [ClientRpc]
    public void RpcMove(int num)
    {
        if (isLocalPlayer)
        {
            return;
        }

        characterMove.MovePressed(num);
    }

    [ClientRpc]
    public void RpcMoveFinished(Vector3 position)
    {
        if (isLocalPlayer)
        {
            return;
        }

        transform.position = position;
    }

    [ClientRpc]
    public void RpcJump(Vector3 position)
    {
        if (isLocalPlayer)
            return;
        transform.position = position;
        jump.JumpPressed();
    }

}


