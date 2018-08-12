using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyManagerHost : MonoBehaviour {

    public NetworkDiscovery networkDiscovery;

	// Use this for initialization
	void Start () {
        if (networkDiscovery == null)
            Debug.Log("no network discovery");
        else
        {
            networkDiscovery.Initialize();
            networkDiscovery.StartAsServer();
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
