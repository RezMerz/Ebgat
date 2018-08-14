using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour {

    [SyncVar] public int clientId;

    private CustomNetworkManager networkManager;
    private ServerManager serverManager;


	// Use this for initialization
	void Start () {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        serverManager = ServerManager.instance;
        Debug.Log(isLocalPlayer);
        /*if(isLocalPlayer){
            serverManager.CmdSpawnMyHero(clientId, networkManager.playerNumber);
        }*/
	}

    private void Update()
    {
        //Debug.Log(isLocalPlayer);
    }

}
