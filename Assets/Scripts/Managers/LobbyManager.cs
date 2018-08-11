using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManager : MonoBehaviour {
    public CustomNetworkDiscovery networkDiscovery;

	// Use this for initialization
	void Start () {
        if (networkDiscovery == null)
            Debug.Log("network discovery is null");
        else
            networkDiscovery.action += OnHostFound;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RefreshButtonPressed(){
        networkDiscovery.StartAsClient();
    }

    public void HostButtonPressed(){
        networkDiscovery.StartAsServer();
    }

    public void OnHostFound(string fromAddress, string data){
        Debug.Log(fromAddress);
    }
}
