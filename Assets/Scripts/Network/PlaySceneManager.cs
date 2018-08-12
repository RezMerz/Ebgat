using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneManager : MonoBehaviour {

    public CustomNetworkDiscovery networkDiscovery;

    public GameObject listItem;
    public GameObject listView;

    // Use this for initialization
    void Start()
    {
        if (networkDiscovery == null)
            Debug.Log("network discovery is null");
        else
            networkDiscovery.action += OnHostFound;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RefreshButtonPressed()
    {
        networkDiscovery.StartAsClient();
    }

    public void HostButtonPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LobbySceneHost");
    }

    public void OnHostFound(string fromAddress, string data)
    {
        Debug.Log(fromAddress);
    }
}
