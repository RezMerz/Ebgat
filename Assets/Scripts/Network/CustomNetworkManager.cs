using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public GameObject serverNetwork;
    public List<GameObject> players;
    private static int num = 1;

    private bool flag = true;

    private void Update()
    {
        Debug.Log("update is calling");
        if(NetworkServer.active && flag){
            flag = false;
            GameObject server = Instantiate(serverNetwork);
            NetworkServer.Spawn(server);
        } 
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        int random = Random.Range(0, players.Count);
        GameObject player = Instantiate(players[random]);
        string team = "Team " + num++;
        Debug.Log(LayerMask.NameToLayer(team));
        player.layer = LayerMask.NameToLayer(team);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        //Debug.Log("server Started");
        //gameObject.AddComponent<ServerNetwork>();
    }


    public override void OnStartHost()
    {
        
        //serverNetwork.SetActive(true);
        base.OnStartHost();
        Debug.Log("host started");

    }
}
