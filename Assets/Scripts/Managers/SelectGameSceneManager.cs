using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGameSceneManager : MonoBehaviour {

    public GameObject textHolder;
    public GameObject buttonSample;
    public Text playerName;
    CustomNetworkDiscovery networkDiscovery;

    private int id;
    private List<ButtonManager> buttonManagers;

    public Vector2 initPosition;

	// Use this for initialization
	void Start () {
        networkDiscovery = GetComponent<CustomNetworkDiscovery>();
        networkDiscovery.action += HostFound;
        buttonManagers = new List<ButtonManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void LoadHostSceen(){
        GameManager.instance.playerName = playerName.text;
        GameManager.instance.currentScene = CurrentScene.LobbyHost;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }

    public void RefreshHosts(){
        if(!networkDiscovery.isClient){
            networkDiscovery.StartAsClient();
        }
    }

    public void HostFound(string fromAddress, string data){
        string ip = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1);
        for (int i = 0; i < buttonManagers.Count; i++){
            if(buttonManagers[i].ip.Equals(ip)){
                return;
            }
        }
        GameObject button = Instantiate(buttonSample, textHolder.transform);
        button.transform.position = initPosition;
        button.GetComponentInChildren<Text>().text = data;
        ButtonManager btnManager = button.GetComponent<ButtonManager>();
        btnManager.SetData(OnHostClicked, id++, ip);
        buttonManagers.Add(btnManager);
        initPosition.x -= 40;
    }

    public void OnHostClicked(int ip){
        GameManager.instance.playerName = playerName.text;
        GameManager.instance.currentScene = CurrentScene.LobbyClient;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }
}
