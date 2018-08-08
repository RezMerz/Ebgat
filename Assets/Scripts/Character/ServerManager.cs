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
        currentWorldState = new WorldState();
    }

    // Use this for initialization
    void Start () {
        SetWorldStateOnPlayers();
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
        PlayerControl[] playerControlArray = new PlayerControl[objs.Length];
        for (int i = 0; i < objs.Length; i++)
        {
            PlayerControl p = objs[i].GetComponent<PlayerControl>();
            playerControlArray[p.clientNetworkSender.PlayerID - 1] = p;
        }
        playerControls.AddRange(playerControlArray);
    }

    public void PlayerSimulationFinished(int ID){
        finishedPLayercounter++;
        if(finishedPLayercounter == playerControls.Count){
            ServerNetworkSender.instance.SendWorldState(currentWorldState);
            finishedPLayercounter = 0;
            currentWorldState = new WorldState();
        }
    }
}
