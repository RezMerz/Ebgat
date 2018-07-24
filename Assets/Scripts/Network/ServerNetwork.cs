using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class ServerNetwork : NetworkBehaviour {

    PlayerControl playerControl;
    ClientNetworkReciever clientNetworkReciever;

	// Use this for initialization
	void Start () {
        playerControl = GetComponent<PlayerControl>();
        clientNetworkReciever = playerControl.clientNetworkReciever;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    [Command]
    public void CmdRecievecommands(string commands){
        string output = ""; 
        string[] lines = commands.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++){
            string[] parts = lines[i].Split(',');
            ECommand cmd = (ECommand)Enum.Parse(typeof(ECommand), parts[0]);
            switch(cmd){
                case ECommand.ShootBullet: CmdShootBullet(new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3])), new Vector3(float.Parse(parts[4]), float.Parse(parts[5]), float.Parse(parts[6])), float.Parse(parts[7])); break;
                default: output += lines + "\n"; break;
            }
        }
        clientNetworkReciever.RpcRecieveCommands(commands);
    }

    [Command]
    public void CmdKillPlayer()
    {
        Destroy(gameObject);
    }

    [Command]
    public void CmdMeleeattack(){
        
    }

    /*[Command]
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
    }*/

    public void CmdShootBullet(Vector3 targetdirection, Vector3 origin, float bulletDamage)
    {
        GameObject bulletObj = Instantiate(playerControl.bulletPrefab);
        NetworkServer.Spawn(bulletObj);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        int layer = LayerMask.GetMask(playerControl.charStats.enemyTeamName, "Blocks");
        bullet.Shoot(targetdirection, origin, bulletDamage, layer);
        bullet.RpcShootBulletForClient(targetdirection, origin, bulletDamage, layer);
    }
}
