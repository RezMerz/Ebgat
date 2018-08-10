using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerManager : NetworkBehaviour {
    
    public static ServerManager instance;
    public WorldState currentWorldState;

    private List<PlayerControl> playerControls;
    private int finishedPLayercounter;

    private void Awake()
    {
        instance = this;
        currentWorldState = new WorldState();
        playerControls = new List<PlayerControl>();
        UpdatePlayers();
    }

    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
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
            ServerNetworkSender.instance.SendWorldState(currentWorldState);
            finishedPLayercounter = 0;
            currentWorldState = new WorldState();
            UpdatePlayers();
        }
    }
}
