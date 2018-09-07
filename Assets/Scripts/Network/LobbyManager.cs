using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyManager : NetworkBehaviour
{

    public Sprite notReadySpriteSample;
    public Sprite readySpriteSample;

    public float timeInterval = 1;
    private float currentTimeInterval;
    CustomNetworkManager networkManager;
    LobbyData[] lobbyData;
    Text[] playerNames;
    Text[] heroNames;
    SpriteRenderer[] readySprite;

    // Use this for initialization
    void Start()
    {
        networkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isServer)
            return;
        currentTimeInterval -= Time.deltaTime;
        if (currentTimeInterval <= 0)
        {
            currentTimeInterval = timeInterval;
            RpcSetLobbyDataOnClients(networkManager.GetLobbyData());
        }
    }

    [ClientRpc]
    public void RpcSetLobbyDataOnClients(string rawData)
    {
        Debug.Log(rawData);
        lobbyData = new LobbyData[6];
        LobbyText lobbyText = GameObject.FindWithTag("LobbyText").GetComponent<LobbyText>();
        playerNames = lobbyText.texts;
        heroNames = lobbyText.heroTexts;
        readySprite = lobbyText.readySprites;
        string[] segment = rawData.Split('$');
        for (int i = 0; i < segment.Length - 1; i++)
        {
            string[] data = segment[i].Split('&');
            int slot = System.Convert.ToInt32(data[0]);
            lobbyData[slot - 1] = new LobbyData(data[1], data[2], data[3]);
        }

        for (int i = 0; i < lobbyData.Length; i++)
        {
            if (lobbyData[i] == null)
            {
                playerNames[i].text = "Empty";
                heroNames[i].text = "No Hero";
                continue;
            }
            playerNames[i].text = lobbyData[i].name;
            heroNames[i].text = lobbyData[i].heroName;
        }
    }

    [ClientRpc]
    public void RpcStartGame()
    {
        Debug.Log("heloooo");
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("Arena");
        /*if (isServer)
        {
            StartCoroutine(StopHostCo(1));

        }
        else
        {
            if (networkManager.isActiveAndEnabled)
            {
                networkManager.StopClient();

            }
        }
        */
    }
}


class LobbyData{
    public string name, heroName;
    public bool isReady;

    public LobbyData(string name, string heroName, string isReady){
        this.name = name;
        this.heroName = heroName;
        if (isReady.Equals("True"))
            this.isReady = true;
        else
            this.isReady = false;
    }

}