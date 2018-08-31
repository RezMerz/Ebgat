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

	// Use this for initialization
	void Start () {
        networkDiscovery = GetComponent<CustomNetworkDiscovery>();
        networkDiscovery.action += HostFound;
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
        Debug.Log("host found");
        string ip = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1);
        bool duplicate = false;
        for (int i = 0; i < buttonManagers.Count; i++){
            if(buttonManagers[i].ip.Equals(ip)){
                duplicate = true;
                break;
            }
        }
        if(!duplicate){
            GameObject button = Instantiate(buttonSample, textHolder.transform);
            button.GetComponentInParent<Text>().text = data;
            ButtonManager btnManager = button.GetComponent<ButtonManager>();
            btnManager.SetData(OnHostClicked, id++, ip);
            buttonManagers.Add(btnManager);
        }
    }

    public void OnHostClicked(int ip){
        
    }
}
