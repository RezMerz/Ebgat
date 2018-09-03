using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Globalization;

public class ClientNetworkReciever : NetworkBehaviour {

    public static ClientNetworkReciever instance;

    private List<PlayerControlClientside> playerControls;
    private PlayerControlClientside localPlayerControl; 

    int playernumber;

    void Awake()
    {
        instance = this;
        playerControls = new List<PlayerControlClientside>();
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
                if(!rawData[2].Equals(""))
                    playerControls[id - 1].Shoot(rawData[2]);
                if(!rawData[3].Equals(""))
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
    public void RpcUpdatePlayers(){
        playernumber = GameObject.FindGameObjectsWithTag("Player").Length;
    }

    private void UpdatePlayer(){
        playerControls.Clear();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        PlayerControlClientside[] playerControlArray = new PlayerControlClientside[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            PlayerControlClientside p = objs[i].GetComponent<PlayerControlClientside>();
            if (p.playerId == 0)
                return;
            playerControlArray[p.playerId - 1] = p;
            if (p.IsLocalPlayer())
                localPlayerControl = p;
        }
        playerControls.AddRange(playerControlArray);
    }

}
