using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour {

    [SyncVar] public int clientId = -1;

    private CustomNetworkManager networkManager;
    private ServerManager serverManager;
    private ClientNetworkSender clientNetworkSender;
    private ServerNetwork serverNetworkReciever;

    public PlayerControlClientside playerControl;
    PlayerControl virtualPlayerControl = null;

    public Vector2 spawnPoint { get; set; }

    [SerializeField]
    private GameObject player;
    private GameObject virtualPlayer;

    public int slot;
    bool first = true;
    string playerName;

	// Use this for initialization
	void Start () {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        serverManager = ServerManager.instance;
        serverNetworkReciever = GetComponent<ServerNetwork>();
        DontDestroyOnLoad(gameObject);
        Debug.Log(isLocalPlayer);
        Debug.Log(hasAuthority);
        if(isLocalPlayer){
            connectionToServer.RegisterHandler(MsgType.Highest + 1, GetAbsoluteState);
            Debug.Log("herhehrerheheheghrerhehrerbdfdfsbbdf");
            networkManager.localPlayerconnection = this;
        }
	}

    private void Update()
    {
        if (first && isLocalPlayer && clientId != -1)
        {
            first = false;
            playerName = GameManager.instance.playerName;
            CmdSetClientDataOnServer(clientId, playerName);
            GameManager.instance.playerConnection = this;
        }
    }

    public void SetClientReady(){
        serverNetworkReciever.CmdClientConnected(clientId);
    }

    public void SetClientNetworkSender(ClientNetworkSender clientNetworkSender){
        this.clientNetworkSender = clientNetworkSender;
    }

    [ClientRpc]
    public void RpcInstansiateHero(int heroId, int teamId, string spawnPoint){
        this.spawnPoint = Toolkit.DeserializeVector(spawnPoint);
        if (isServer)
        {
            player = Instantiate(networkManager.clientSidePlayers[heroId], this.spawnPoint, Quaternion.Euler(0, 0, 0));
            virtualPlayer = Instantiate(networkManager.serverSidePlayers[heroId], this.spawnPoint, Quaternion.Euler(0, 0, 0));
            virtualPlayerControl = virtualPlayer.GetComponent<PlayerControl>();
            virtualPlayerControl.SetNetworkComponents(this, clientNetworkSender, serverNetworkReciever, clientId);
            SetVirtualTeam(teamId);
        }
        else
            player = Instantiate(networkManager.clientSidePlayers[heroId]);
        playerControl = player.GetComponent<PlayerControlClientside>();
        serverNetworkReciever.SetPlayerControl(virtualPlayerControl);
        playerControl.SetNetworkComponents(this, clientNetworkSender, serverNetworkReciever, clientId);
        SetTeam(teamId);
        StartCoroutine(SetReadyWait(1));
        //serverNetworkReciever.CmdHeroSpawned(clientId);
    }

    private void SetTeam(int teamId){
        string teamName = "", enemyTeamName = "";
        if(teamId == 1){
            teamName = "Team " + 1;
            enemyTeamName = "Team " + 2;
        }
        else if(teamId == 2){
            teamName = "Team " + 2;
            enemyTeamName = "Team " + 1;
        }
        else{
            Debug.Log("wrong team id");
        }
        playerControl.SetTeam(teamName, enemyTeamName);
    }

    private void SetVirtualTeam(int teamId){
        string teamName = "", enemyTeamName = "";
        if (teamId == 1)
        {
            teamName = "Virtual Team " + 1;
            enemyTeamName = "Virtual Team " + 2;
        }
        else if (teamId == 2)
        {
            teamName = "Virtual Team " + 2;
            enemyTeamName = "Virtual Team " + 1;
        }
        else
        {
            Debug.Log("wrong team id");
        }
        virtualPlayerControl.SetTeam(teamName, enemyTeamName);
    }

    [ClientRpc]
    public void RpcSetReady(){
        playerControl.SetReady();
    }

    private IEnumerator SetReadyWait(int time){
        yield return new WaitForSeconds(time);
        playerControl.SetReady();
        //yield return new WaitForSeconds(time);
        if (isServer)
            virtualPlayerControl.SetReady();
    }

    public void GetAbsoluteState(NetworkMessage netMsg){
        Debug.Log("recieved");
        int a = netMsg.reader.ReadInt32();
        string s = netMsg.reader.ReadString();
        playerControl.clientNetworkReciever.RecieveWorldstate(s, a);
    }

    public void SendAbsoluteState(AbsoluteStateMessage message){
        base.connectionToClient.Send(MsgType.Highest + 1, message);
    }

    [ClientRpc]
    public void RpcKillHero(){
        playerControl.Die();
    }

    [ClientRpc]
    public void RpcRespawnHero(){
        playerControl.Respawn();
    }

    [ClientRpc]
    public void RpcGameFinished(int winnerTeamId){
        Debug.Log("Team " + winnerTeamId + "Won the game");
        if (winnerTeamId == 1)
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Team1 Win");
        else if (winnerTeamId == 2)
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Team2 Win");
    }

    public void ChangeTeamClicked()
    {
        CmdChangeMyTeam(clientId);
    }

    [Command]
    public void CmdSetClientDataOnServer(int id, string name)
    {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        networkManager.SetClientDataOnServer(id, name);
    }

    [Command]
    public void CmdChangeMyTeam(int clientId)
    {
        networkManager.ChangeTeam(clientId);
    }
}
