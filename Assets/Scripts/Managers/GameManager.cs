using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public CurrentScene currentScene { get; set; }
    public string playerName;

    CustomNetworkDiscovery networkDiscovery;

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
            currentScene = CurrentScene.Menu;
        }
        else{
            if (instance.currentScene == CurrentScene.LobbyHost)
                StartLobbyHost();
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
	}
	

	// Update is called once per frame
	void Update () {
        
	}

    void StartLobbyHost(){
        networkDiscovery = GameObject.FindWithTag("Network Manager").GetComponent<CustomNetworkDiscovery>();
        networkDiscovery.StartAsServer();
    }
}

public enum CurrentScene{
    Menu, LobbyHost, LobbyClient, Game
}