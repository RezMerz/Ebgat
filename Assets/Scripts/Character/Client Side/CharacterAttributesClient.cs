using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class CharacterAttributesClient : MonoBehaviour {
    
    private PlayerControl playerControl;
    public string teamName, enemyTeamName;
    public Vector2 side;
    
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerControl.ReadyAction += SetReady;
    }

    public void SetReady(){
      
    }
}
