using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Globalization;

public class ServerNetwork : NetworkBehaviour {

    public static ServerNetwork instance;


    public List<PlayerControl> playerControls;// { get; set;}
    ClientNetworkReciever clientNetworkReciever;
    string data = "";

    private void Awake()
    {
        instance = this;
        playerControls = new List<PlayerControl>();

    }

    // Use this for initialization
    void Start () {
        clientNetworkReciever = ClientNetworkReciever.instance;
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
    public void CmdRecievecommands(string commands, int playerID){
        string[] lines = commands.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++){
            string[] parts = lines[i].Split(',');
            switch(parts[0]){
                case "1": playerControls[playerID - 1].MoveRight(); break;
                case "2": playerControls[playerID - 1].MoveLeft(); break;
                case "3": playerControls[playerID - 1].MoveFinished(new Vector3(float.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[3], CultureInfo.InvariantCulture.NumberFormat))); break;
                case "7": playerControls[playerID - 1].SetVerticalDirection(Convert.ToInt32(parts[1])); break;
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

    /*public void CmdShootBullet(Vector3 targetdirection, Vector3 origin, float bulletDamage)
    {
        GameObject bulletObj = Instantiate(playerControl.bulletPrefab);
        NetworkServer.Spawn(bulletObj);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        int layer = LayerMask.GetMask(playerControl.charStats.enemyTeamName, "Blocks");
        bullet.Shoot(targetdirection, origin, bulletDamage, layer);
        bullet.RpcShootBulletForClient(targetdirection, origin, bulletDamage, layer);
    }*/


    public void ClientMove(int playerID, Vector3 position){
        data += playerID + "," + 1 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void ClientMoveFinished(int playerID, Vector3 position){
        data += playerID + "," + 2 + "," + position.x + "," + position.y + "," + position.z + ",\n";
    }

    public void ClientSetVerticalSide(int playerID, int num){
        data += playerID + "," + 6 + "," + num + ",\n";
    }
}
