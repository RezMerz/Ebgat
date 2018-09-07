using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkDosciveryEnabler : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
        //GetComponent<CustomNetworkDiscovery>().StartAsServer();
        Debug.Log("starting host");
        GetComponent<LobbyNetworkManager>().StartHost();
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void start(){

        //GetComponent<LobbyNetworkManager>().StartGame();
    }
}
