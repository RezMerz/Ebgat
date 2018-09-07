using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour {
    public static Music instance; 
    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Arena 2")
            Destroy(Music.instance);

        if (instance == null)
            instance = null;
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
