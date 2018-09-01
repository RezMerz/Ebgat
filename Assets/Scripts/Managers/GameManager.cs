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
    public LobbyClient myLobbyClient { get; set; }

	// Use this for initialization
	void Awake () {
        if (instance == null)
        {
            instance = this;
            currentScene = CurrentScene.Menu;
            /*if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name.Equals("LobbyScene")){
                UnityEngine.SceneManagement.SceneManager.LoadScene("GameSelectScene");
            }*/
                
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

    public void OnChangeTeamClicked(){
        myLobbyClient.ChangeTeamClicked();
    }

    public void OnLeaveClicked(){
        
    }

    public void OnStartClicked(){
        Debug.Log("on start cliiiiiiiiicked");
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<LobbyNetworkManager>();
        networkManager.StartGame();
    }

    void StartLobbyHost(){
        networkDiscovery = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkDiscovery>();
        if (playerName.Length == 0)
            playerName = "Player";
        networkDiscovery.broadcastData = playerName;
        //networkDiscovery.StartAsServer();
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<LobbyNetworkManager>();
        networkManager.StartHost();
    }

    void StartLobbyClient(){
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<LobbyNetworkManager>();
        networkManager.networkAddress = instance.hostIp;
        networkManager.StartClient();
    }
}

public enum CurrentScene{
    Menu, LobbyHost, LobbyClient, Game
}