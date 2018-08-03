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
        string team = "Team " + num++;
        Debug.Log(LayerMask.NameToLayer(team));
        player.layer = LayerMask.NameToLayer(team);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
