﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour {

    public float timeInterval = 1;
    private float currentTimeInterval;
    LobbyNetworkManager networkManager;
    string[] lobbyData;
    Text[] playerNames;

	// Use this for initialization
	void Start () {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<LobbyNetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isServer)
            return;
        currentTimeInterval -= Time.deltaTime;
        if (currentTimeInterval <= 0)
        {
            currentTimeInterval = timeInterval;
            RpcSetLobbyDataOnClients(networkManager.GetLobbyData());
        }
	}

    [ClientRpc]
    public void RpcSetLobbyDataOnClients(string rawData){
        lobbyData = new string[6];
        playerNames = GameObject.FindWithTag("LobbyText").GetComponent<LobbyText>().texts;
        string[] segment = rawData.Split('$');
        for (int i = 0; i < segment.Length - 1; i++){
            Debug.Log(segment[i]);
            string[] data = segment[i].Split('&');
            int slot = System.Convert.ToInt32(data[0]);
            lobbyData[slot - 1] = data[1];
        }

        for (int i = 0; i < lobbyData.Length; i++){
            if (lobbyData[i] == null)
                playerNames[i].text = "Empty";
            else
                playerNames[i].text = lobbyData[i];
        }
    }
}