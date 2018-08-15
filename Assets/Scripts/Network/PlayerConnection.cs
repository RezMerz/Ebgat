using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour {

    [SyncVar] public int clientId;

    private CustomNetworkManager networkManager;
    private ServerManager serverManager;
    private ClientNetworkSender clientNetworkSender;
    private ServerNetwork serverNetworkReciever;

    [SerializeField]
    private GameObject player;

	// Use this for initialization
	void Start () {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        serverManager = ServerManager.instance;
        clientNetworkSender = GetComponent<ClientNetworkSender>();
        serverNetworkReciever = GetComponent<ServerNetwork>();
        Debug.Log(isLocalPlayer, gameObject);
        if(isLocalPlayer){
            serverNetworkReciever.CmdClientConnected(clientId, networkManager.playerNumber);
        }
	}

    private void Update()
    {
        //Debug.Log(isLocalPlayer);
    }

    [ClientRpc]
    public void RpcInstansiatePlayer(int heroId){
        player = Instantiate(networkManager.players[heroId], transform);
        serverNetworkReciever.SetPlayerControl(player.GetComponent<PlayerControl>());
    }

}
