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

    public float respawnTime, respawnPenalty;
    public bool isInfinite;

    private float resTimeTeam1;
    private float resTimeTeam2;
    private int team1Count = 0, team1DeadCount = 0;
    private int team2Count = 0, team2DeadCount = 0;

    public void Awake()
    {
        instance = this;
        playerControls = new List<PlayerControl>();
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        reservelist = new List<int>();
        playerInfoList = new List<PlayerInfo>();
        deadPlayers = new List<PlayerInfo>();
        maxClientCount = networkManager.maxPlayerCount;
        respawnTime = networkManager.baseRespawnTime;
        respawnPenalty = networkManager.respawnTimePenalty;
        resTimeTeam1 = respawnTime;
        resTimeTeam2 = respawnTime;
        isInfinite = networkManager.isInfinite;
        currentClientCount = 0;
        UpdatePlayers();
    }

    // Use this for initialization
    void Start () {
        currentWorldState = new WorldState();
	}

    private void FixedUpdate()
    {
        
        for (int i = 0; i < deadPlayers.Count; i++){
            deadPlayers[i].respawnTimeLeft -= Time.fixedDeltaTime;
            if(deadPlayers[i].respawnTimeLeft <= 0){
                if (deadPlayers[i].teamId == 1)
                    team1DeadCount--;
                if (deadPlayers[i].teamId == 2)
                    team2DeadCount--;
                RespawnHero(deadPlayers[i]);
                for (int j = 0; j < playerControls.Count; j++){
                    if (playerControls[j].playerId == deadPlayers[i].clientId)
                        playerControls[j].Respawn();
                        
                }
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
        GameObject[] objs = GameObject.FindGameObjectsWithTag("VirtualPlayer");
        for (int i = 0; i < objs.Length; i++)
            playerControls.Add(objs[i].GetComponent<PlayerControl>());
        SetWorldStateOnPlayers();
    }

    public void PlayerSimulationFinished(int ID){
        if(currentWorldState == null)
            currentWorldState = new WorldState();
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
            p.charStats.RegisterAllStates(tempWorldState);
        }
        return tempWorldState;
    }
    
    public void SendWorldStateToClient(int playerID){
        WorldState tempWorldState = new WorldState();
        foreach (PlayerControl p in playerControls)
        {
            tempWorldState.RegisterHeroPhysics(p.playerId, p.physic.virtualPosition, Vector2.zero);
            p.charStats.RegisterAllStates(tempWorldState);
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
            {
                team1Count++;
                teamId = 2;
            }
            else
            {
                team2Count++;
                teamId = 1;
            }
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
                deadPlayers.Add(playerInfoList[i]);
                if (playerInfoList[i].teamId == 1)
                {
                    team1DeadCount++;
                    resTimeTeam1 += respawnPenalty;
                    playerInfoList[i].respawnTimeLeft = resTimeTeam1;
                }
                else
                {
                    team2DeadCount++;
                    resTimeTeam2 += respawnPenalty;
                    playerInfoList[i].respawnTimeLeft = resTimeTeam2;
                }
                SendKillCommand(playerId);
                if (team1DeadCount > 0 && team1Count == team1DeadCount)
                {
                    if (!isInfinite)
                    {
                        Debug.Log("team 2 wins");
                        SendGameFinishedCommand(2);
                    }
                }
                else if (team2DeadCount > 0 && team2Count == team2DeadCount)
                {
                    if (!isInfinite)
                    {
                        Debug.Log("team 1 wins");
                        SendGameFinishedCommand(1);
                    }
                }
                break;

            }
        }
    }

    public void SendGameFinishedCommand(int winnerTeamId){
        for (int i = 0; i < networkManager.playerConnections.Count; i++){
            networkManager.playerConnections[i].RpcGameFinished(winnerTeamId);
        }
    }

    public void SendKillCommand(int playerId){
        currentWorldState.AdditionalPlayerData(playerId, "Z");
    }

    private void RespawnHero(PlayerInfo playerInfo){
        playerInfo.isAlive = true;
        currentWorldState.AdditionalPlayerData(playerInfo.clientId, "Y");
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