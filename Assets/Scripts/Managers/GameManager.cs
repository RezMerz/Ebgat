using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public CurrentScene currentScene { get; set; }
    public string playerName;

    CustomNetworkDiscovery networkDiscovery;
    LobbyNetworkManager networkManager;

    public string hostIp { get; set; }

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
            currentScene = CurrentScene.Menu;
        }
        else{
            if (instance.currentScene == CurrentScene.LobbyHost)
                instance.StartLobbyHost();
            else if (instance.currentScene == CurrentScene.LobbyClient)
                StartLobbyClient();
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}
	

	// Update is called once per frame
	void Update () {
        
	}

    void StartLobbyHost(){
        networkDiscovery = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkDiscovery>();
        networkDiscovery.broadcastData = playerName;
        networkDiscovery.StartAsServer();
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<LobbyNetworkManager>();
        networkManager.StartServer();
    }

    void StartLobbyClient(){
        Debug.Log(hostIp);
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<LobbyNetworkManager>();
        networkManager.networkAddress = instance.hostIp;
        networkManager.StartClient();
    }
}

public enum CurrentScene{
    Menu, LobbyHost, LobbyClient, Game
}