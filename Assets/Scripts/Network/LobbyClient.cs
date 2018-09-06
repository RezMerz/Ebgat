using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyClient : NetworkBehaviour {

    [SyncVar] public int id;
    public int slot;
    bool first = true;
    string playerName;
    CustomNetworkManager networkManager;

    private void Update()
    {
        if(first && isLocalPlayer){
            first = false;
            playerName = GameManager.instance.playerName;
            CmdSetClientDataOnServer(id, playerName);
        }
    }

    public void ChangeTeamClicked(){
        CmdChangeMyTeam(id);
    }

    [Command]
    public void CmdSetClientDataOnServer(int id, string name){
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        networkManager.SetClientDataOnServer(id, name);
    }

    [Command]
    public void CmdChangeMyTeam(int clientId){
        networkManager.ChangeTeam(clientId);
    }
}
