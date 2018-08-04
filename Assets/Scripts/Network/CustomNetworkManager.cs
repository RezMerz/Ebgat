using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {

    public List<GameObject> players;

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        int random = Random.Range(0, players.Count);
        GameObject player = Instantiate(players[random]);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
    }
}
