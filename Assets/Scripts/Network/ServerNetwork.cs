using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerNetwork : NetworkBehaviour {

    PlayerControl playerControl;
    ClientNetworkReciever clientNetworkReciever;

	// Use this for initialization
	void Start () {
        playerControl = GetComponent<PlayerControl>();
        clientNetworkReciever = playerControl.clientNetwork
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Command]
    public void CmdRecievecommands(string commands){
        
    }

    [Command]
    public void CmdKillPlayer()
    {
        Destroy(gameObject);
    }


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
    public void CmdJumpPressed(Vector3 position)
    {
        RpcJumpPressed(position);
    }

    [Command]
    public void CmdJumpHold(Vector3 position)
    {
        RpcJumpHold(position);
    }

    [Command]
    public void CmdJumpReleased(Vector3 position)
    {
        RpcJumpReleased(position);
    }

    [Command]
    public void CmdShootbullet(Vector3 targetdirection, Vector3 origin, float bulletDamage)
    {
        GameObject bulletObj = Instantiate(bulletPrefab);
        NetworkServer.Spawn(bulletObj);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        int layer = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
        bullet.Shoot(targetdirection, origin, bulletDamage, layer);
        bullet.RpcShootBulletForClient(targetdirection, origin, bulletDamage, layer);
    }
}
