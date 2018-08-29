using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : NetworkBehaviour {
    
    public static ServerManager instance;
    public WorldState currentWorldState;

    private CustomNetworkManager networkManager;
    private List<PlayerControl> playerControls;
    private int finishedPLayercounter;

    public int CurrentStateID = 0;

    private List<int> reservelist;

    private List<PlayerInfo> playerInfoList;
    private List<PlayerInfo> deadPlayers;

    public int maxClientCount, currentClientCount, spawnedHeroCount;
    private int bulletIdCounter = 0;

    public float respawnTime = 5f;

    public void Awake()
    {
        instance = this;
        currentWorldState = new WorldState();
        playerControls = new List<PlayerControl>();
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        reservelist = new List<int>();
        playerInfoList = new List<PlayerInfo>();
        deadPlayers = new List<PlayerInfo>();
        maxClientCount = networkManager.maxPlayerCount;
        currentClientCount = 0;
        UpdatePlayers();
    }

    // Use this for initialization
    void Start () {
	}

    private void FixedUpdate()
    {
        for (int i = 0; i < deadPlayers.Count; i++){
            deadPlayers[i].respawnTimeLeft -= Time.fixedDeltaTime;
            if(deadPlayers[i].respawnTimeLeft <= 0){
                //Debug.Log();
                RespawnHero(deadPlayers[i]);
                deadPlayers.RemoveAt(i);
                i--;
            }
        }
    }

    private void SetWorldStateOnPlayers(){
        foreach(PlayerControl p in playerControls){
            p.worldState = currentWorldState;
        }
    }

    public void UpdatePlayers()
    {
        playerControls.Clear();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < objs.Length; i++)
            playerControls.Add(objs[i].GetComponent<PlayerControl>());
        SetWorldStateOnPlayers();
    }

    public void PlayerSimulationFinished(int ID){
        finishedPLayercounter++;
        if(finishedPLayercounter == playerControls.Count){
            ServerNetworkSender.instance.RegisterWorldState(currentWorldState);
            finishedPLayercounter = 0;
            currentWorldState = new WorldState();
            UpdatePlayers();

        }
    }

    //temp
    public WorldState GetFullState(){
        WorldState tempWorldState = new WorldState();
        foreach (PlayerControl p in playerControls)
        {
            tempWorldState.RegisterHeroPhysics(p.playerId, p.physic.virtualPosition, Vector2.zero);
            p.worldState = tempWorldState;
            p.charStats.RegisterAllStates();
        }
        return tempWorldState;
    }
    
    public void SendWorldStateToClient(int playerID){
        WorldState tempWorldState = new WorldState();
        foreach (PlayerControl p in playerControls)
        {
            tempWorldState.RegisterHeroPhysics(p.playerId, p.physic.virtualPosition, Vector2.zero);
            p.charStats.RegisterAllStates();
        }
        ServerNetworkSender.instance.SendWorldFullstate(tempWorldState, playerID);
    }

    public void SpawnHero(int clientId, int heroId, int teamId, Vector2 SpawnPoint)
    {
        for (int i = 0; i < networkManager.playerConnections.Count; i++)
        {
            if (networkManager.playerConnections[i].clientId == clientId)
            {
                networkManager.playerConnections[i].RpcInstansiateHero(heroId, teamId, Toolkit.VectorSerialize(SpawnPoint));
                break;
            }
        }

    }


    public void ClientConnected(int clientId, int heroId)
    {
        Debug.Log("id: " + clientId + " , " + heroId);
        playerInfoList.Add(new PlayerInfo(clientId, heroId, true));
        currentClientCount++;

    }

    public void StartGame()
    {
        int teamId = 1;
        networkManager.gameObject.GetComponent<CustomNetworkDiscovery>().StopBroadcast();
        for (int i = 0; i < currentClientCount; i++)
        {
            SpawnHero(playerInfoList[i].clientId, playerInfoList[i].heroId, teamId, networkManager.heroSpawnPositions[teamId - 1].position);
            playerInfoList[i].teamId = teamId;
            if (teamId == 1)
                teamId = 2;
            else
                teamId = 1;
        }
        UpdatePlayers();
        ClientNetworkReciever.instance.RpcUpdatePlayers();
    }

    public void HeroSpawned(int cliendId){
        spawnedHeroCount++;
        if(spawnedHeroCount == maxClientCount){ //spawn at the begining of the match
            for (int i = 0; i < maxClientCount; i++){
                networkManager.playerConnections[i].RpcSetReady();
            }
            spawnedHeroCount++; //for respawin
        }
        else if(spawnedHeroCount > maxClientCount){ //respawn
            for (int i = 0; i < maxClientCount; i++)
            {
                if (networkManager.playerConnections[i].clientId == cliendId)
                {
                    networkManager.playerConnections[i].RpcSetReady();
                    break;
                }
            }
        }
    }

    public int GetBulletID(int playerId)
    {
        return ++bulletIdCounter;
    }

    public void KillHero(int playerId){
        for (int i = 0; i < playerInfoList.Count; i++)
        {
            if (playerInfoList[i].clientId == playerId)
            {
                playerInfoList[i].isAlive = false;
                playerInfoList[i].respawnTimeLeft = respawnTime;
                deadPlayers.Add(playerInfoList[i]);
                SendKillCommand(playerId);
                break;

            }
        }
    }

    public void SendKillCommand(int playerId){
        for (int i = 0; i < networkManager.playerConnections.Count; i++)
        {
            if (networkManager.playerConnections[i].clientId == playerId)
            {
                networkManager.playerConnections[i].RpcKillHero();
                break;
            }
        }
    }

    private void RespawnHero(PlayerInfo playerInfo){
        playerInfo.isAlive = true;
        for (int i = 0; i < networkManager.playerConnections.Count; i++)
        {
            if (networkManager.playerConnections[i].clientId == playerInfo.clientId)
            {
                networkManager.playerConnections[i].RpcRespawnHero();
            }
        }
    }
}

class PlayerInfo{
    public int clientId, heroId, teamId;
    public bool isAlive;
    public float respawnTimeLeft;
    public PlayerConnection playerConnection;

    public PlayerInfo(int clientId, int heroId, bool isAlive){
        this.heroId = heroId;
        this.clientId = clientId;
        this.isAlive = isAlive;
        respawnTimeLeft = 0;
        teamId = 0;
    }
}