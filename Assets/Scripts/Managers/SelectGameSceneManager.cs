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
    public GameObject butonParent;
    private int counter = 0;
	// Use this for initialization
	void Start () {
        networkDiscovery = GetComponent<CustomNetworkDiscovery>();
        networkDiscovery.action += HostFound;
        buttonManagers = new List<ButtonManager>();
	}
	
    public void BackToMainMenu(){
        CustomNetworkDiscovery customNetworkDiscovery = GetComponent<CustomNetworkDiscovery>();
        if (customNetworkDiscovery.isClient)
            customNetworkDiscovery.StopBroadcast();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main Menu");
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
        Debug.Log("host found");
        GameObject button = butonParent.transform.GetChild(counter).gameObject;
        button.SetActive(true);
        string hostName = data + " Game";
        button.GetComponentInChildren<Text>().text = hostName;
        ButtonManager btnManager = button.GetComponent<ButtonManager>();
        btnManager.SetData(OnHostClicked, id++, ip);
        buttonManagers.Add(btnManager);
        counter++;
    }

    public void OnHostClicked(string ip){
        GameManager.instance.playerName = playerName.text;
        if (GameManager.instance.playerName.Length == 0)
            GameManager.instance.playerName = "Player";
        GameManager.instance.currentScene = CurrentScene.LobbyClient;
        GameManager.instance.hostIp = ip;
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbyScene");
    }
}
