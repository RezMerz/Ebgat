using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {
    
    public GameObject serverNetwork;
    public GameObject clientNetworkReciever;
    public List<GameObject> players;
    public GameObject player;

    private bool flag = true;

    private void Update()
    {
        if(NetworkServer.active && flag){
            flag = false;
            GameObject server = Instantiate(serverNetwork);
            GameObject clientNetwork = Instantiate(clientNetworkReciever);
            NetworkServer.Spawn(clientNetwork);
            NetworkServer.Spawn(server);
        } 
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        int random = Random.Range(0, players.Count);
        player = Instantiate(players[random]);
        ClientNetworkReciever.instance.playerControls.Add(player.GetComponent<PlayerControl>());
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }


    [ClientRpc]
    public void AddPlayerComponent(){
        ClientNetworkReciever.instance.playerControls.Add(player.GetComponent<PlayerControl>());
    }
}
