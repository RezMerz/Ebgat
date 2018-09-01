﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyClient : NetworkBehaviour {

    public int id, slot;
    bool first = true;
    string playerName;
    LobbyNetworkManager networkManager;

    private void Update()
    {
        if(first && isLocalPlayer){
            first = false;
            name = GameManager.instance.playerName;
            CmdSetClientDataOnServer(id, name);
        }
    }

    [Command]
    public void CmdSetClientDataOnServer(int id, string name){
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<LobbyNetworkManager>();
        networkManager.SetClientDataOnServer(id, name);
    }
}
