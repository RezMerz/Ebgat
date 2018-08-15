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



    private void Awake()
    {
        instance = this;
        currentWorldState = new WorldState();
        playerControls = new List<PlayerControl>();
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
        reservelist = new List<int>();
        UpdatePlayers();
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	/*void Update () {
        if (playerControls.Count == 0)
            UpdatePlayers();
	}*/

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

    public void RequestworldFullState(int playerID){
        reservelist.Add(playerID);
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

    public void SendFullWorldStates(){
        foreach(int id in reservelist){
            SendWorldStateToClient(id);
        }
        reservelist.Clear();
    }


    [Command]
    public void CmdSpawnMyHero(int clientId, int heroId)
    {
        SpawnHero(clientId, heroId);
    }

    public void SpawnHero(int clientId, int heroId)
    {
        for (int i = 0; i < networkManager.playerConnections.Count; i++)
        {
            if (networkManager.playerConnections[i].clientId == clientId)
            {
                Debug.Log("fack");
                networkManager.playerConnections[i].RpcInstansiatePlayer(heroId);
                ServerManager.instance.UpdatePlayers();
                ClientNetworkReciever.instance.RpcUpdatePlayers();
                break;
            }
        }

    }

}
