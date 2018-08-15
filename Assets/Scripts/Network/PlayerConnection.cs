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
    private PlayerControl playerControl;

    [SerializeField]
    private GameObject player;

	// Use this for initialization
	void Start () {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        serverManager = ServerManager.instance;
        serverNetworkReciever = GetComponent<ServerNetwork>();
        if(isLocalPlayer){
            serverNetworkReciever.CmdClientConnected(clientId, networkManager.playerNumber);
        }
	}

    private void Update()
    {
        //Debug.Log(isLocalPlayer);
    }

    public void SetClientNetworkSender(ClientNetworkSender clientNetworkSender){
        this.clientNetworkSender = clientNetworkSender;
    }

    [ClientRpc]
    public void RpcInstansiateHero(int heroId){
        player = Instantiate(networkManager.players[heroId]);
        playerControl = player.GetComponent<PlayerControl>();
        serverNetworkReciever.SetPlayerControl(playerControl);
        playerControl.SetNetworkComponents(this, clientNetworkSender, serverNetworkReciever, clientId);
        StartCoroutine(SetReadyWait(1));
        //serverNetworkReciever.CmdHeroSpawned(clientId);
    }

    [ClientRpc]
    public void RpcSetReady(){
        playerControl.SetReady();
    }

    private IEnumerator SetReadyWait(int time){
        yield return new WaitForSeconds(time);
        playerControl.SetReady();
    }
}
