using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager {
    
    public GameObject serverNetwork;
    public GameObject clientNetworkReciever;
    public List<GameObject> players;

    private bool flag = true;
    bool start = true;
    public int playerNumber { get; set; }



    private void Update()
    {
        /*if(Input.GetKeyDown(KeyCode.Space)){
            start = false;
            Debug.Log("hello");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
        }
        Debug.Log(Application.targetFrameRate);*/
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
        GameObject player = Instantiate(players[playerNumber]);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        ServerManager.instance.UpdatePlayers();
        ClientNetworkReciever.instance.RpcUpdatePlayers();
    }

    public override void OnServerReady(NetworkConnection conn)
    {
        base.OnServerReady(conn);

    }




    //[Command]
}
