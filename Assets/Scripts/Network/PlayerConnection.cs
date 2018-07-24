using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour {

    public List<GameObject> players;

	// Use this for initialization
	void Start () {
        if (!isLocalPlayer)
            return;
        CmdSpawnMyUnit();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    [Command]
    public void CmdSpawnMyUnit(){
        int random = Random.Range(0, players.Count);
        GameObject player = Instantiate(players[random]);
        NetworkServer.SpawnWithClientAuthority(player, gameObject);
    }
}
