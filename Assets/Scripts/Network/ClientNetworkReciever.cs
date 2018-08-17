using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Globalization;

public class ClientNetworkReciever : NetworkBehaviour {

    public static ClientNetworkReciever instance;

    private List<PlayerControl> playerControls;
    private PlayerControl localPlayerControl; 

    int playernumber;

    void Awake()
    {
        instance = this;
        playerControls = new List<PlayerControl>();
    }

    [ClientRpc]
    public void RpcRecieveWorldData(string[] worlddata, int packetID){
        if (playerControls.Count != playernumber)
            UpdatePlayer();
        for (int i = 0; i < worlddata.Length; i++)
        {
            if (worlddata[i].Length == 0)
                continue;
            string[] heroData = worlddata[i].Split('#');
            int frameBaseId = packetID * 3;
            for (int j = 0; j < heroData.Length; j++)
            {
                if (heroData[j].Length == 0)
                    continue;
                string[] rawData = heroData[j].Split('@');
                string[] data = rawData[1].Split('$');
                int id = Convert.ToInt32(rawData[0]);
                if (id == 0 || id > playerControls.Count)
                    continue;
                playerControls[id - 1].AddTOHashTable(frameBaseId + i, rawData[1]);
                playerControls[id - 1].Shoot(rawData[2]);
                playerControls[id - 1].DestroyBullet(rawData[3]);
            }
        }
    }

    [ClientRpc]
    public void RpcRecieveWorldstate(string worldstate, int frameID, int RequesterID){
        if (localPlayerControl.playerId != RequesterID)
            return;
        if (worldstate.Length == 0)
            return;
        string[] heroData = worldstate.Split('#');
        for (int j = 0; j < heroData.Length; j++)
        {
            if (heroData[j].Length == 0)
                continue;
            string[] rawData = heroData[j].Split('@');
            string[] data = rawData[1].Split('$');
            int id = Convert.ToInt32(rawData[0]);
            if (id == 0 || id > playerControls.Count)
                continue;
            playerControls[id - 1].UpdateClient(frameID, rawData[1]);
        }
    }

    [ClientRpc]
    public void RpcRecieveCommands(string data, string hitdata){
        string[] lines = data.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] parts = lines[i].Split(',');
            int playerID = Convert.ToInt32(parts[0]);
            switch (parts[1])
            {
                //case "1": playerControls[playerID - 1].characterMove.MoveClientside(new Vector3(float.Parse(parts[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[3], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[4], CultureInfo.InvariantCulture.NumberFormat))); break;
                //case "2": playerControls[playerID - 1].characterMove.MoveReleasedClientside(new Vector3(float.Parse(parts[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[3], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[4], CultureInfo.InvariantCulture.NumberFormat))); break;
                case "6": playerControls[playerID - 1].SetVerticalDirection(Convert.ToInt32(parts[2])); break;
                case "7": playerControls[playerID - 1].attack.AttackClientside(new Vector2(float.Parse(parts[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[3], CultureInfo.InvariantCulture.NumberFormat)), Convert.ToInt32(parts[4])); break;
              //  case "8": playerControls[playerID - 1].attack.AttackHitClientSide(Convert.ToInt32(parts[2])); break;
                case "9": playerControls[playerID - 1].TakeAttack(float.Parse(parts[2], CultureInfo.InvariantCulture.NumberFormat), parts[3]); break;
               // case "10": playerControls[playerID - 1].attack.AttackClientside(new Vector2(float.Parse(parts[2], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[3], CultureInfo.InvariantCulture.NumberFormat)), Convert.ToInt32(parts[4])); break;
                default: Debug.Log("wrong data"); break;
            }
        }
        lines = hitdata.Split('\n');
        for (int i = 0; i < lines.Length - 1; i++)
        {
            string[] parts = lines[i].Split(',');
            switch (parts[0])
            {
                default: Debug.Log("wrong hit data"); break;
            }
        }
    }

    [ClientRpc]
    public void RpcUpdatePlayers(){
        playernumber = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    private void UpdatePlayer(){
        playerControls.Clear();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        PlayerControl[] playerControlArray = new PlayerControl[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            PlayerControl p = objs[i].GetComponent<PlayerControl>();
            if (p.playerId == 0)
                return;
            playerControlArray[p.playerId - 1] = p;
            if (p.IsLocalPlayer())
                localPlayerControl = p;
        }
        playerControls.AddRange(playerControlArray);
    }

}
