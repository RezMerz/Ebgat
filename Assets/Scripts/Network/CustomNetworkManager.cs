using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public List<GameObject> players;
    private static int num = 1;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        int random = Random.Range(0, players.Count);
        GameObject player = Instantiate(players[random]);
        string team = "Team" + num++;
        Debug.Log(LayerMask.GetMask(team));
        player.layer = LayerMask.GetMask(team); 
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
