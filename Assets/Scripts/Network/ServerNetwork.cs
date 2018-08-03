using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Globalization;

public class ServerNetwork : NetworkBehaviour {

    private static int PlayerID = 1;

    PlayerControl playerControl;
    ClientNetworkReciever clientNetworkReciever;
    string data = ""; 
	// Use this for initialization
	void Start () {
        playerControl = GetComponent<PlayerControl>();
        clientNetworkReciever = playerControl.clientNetworkReciever;
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
            return;
        if (data.Equals(""))
            return;
        SendCommands();
	}

    private void SendCommands()
    {
        clientNetworkReciever.RpcRecieveCommands(data);
        data = "";
    }

    [Command]
    public int CmdGetplayerID(){
        return PlayerID ++;
    }

    [Command]
    public void CmdRecievecommands(string commands){
        string[] lines = commands.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++){
            string[] parts = lines[i].Split(',');
            switch(parts[0]){
                case "1": playerControl.MoveRight(); break;
                case "2": playerControl.MoveLeft(); break;
                case "3": playerControl.MoveFinished(new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[3], CultureInfo.InvariantCulture.NumberFormat))); break;
                case "7": playerControl.SetVerticalDirection(Convert.ToInt32(parts[1])); break;
            }
        }
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


    public void ClientMove(Vector3 position){
        data += 1 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void ClientMoveFinished(Vector3 position){
        data += 2 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void ClientSetVerticalSide(int num){
        data += 6 + "," + num + ",\n";
    }
}
