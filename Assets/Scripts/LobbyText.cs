using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyText : MonoBehaviour {

    public Text[] texts;


    public void OnChangeTeamClick(){
        GameManager.instance.OnChangeTeamClicked();
    }

    public void OnStartClicked(){
        GameManager.instance.OnStartClicked();
    }

    public void OnLeaveClicked(){
        
    }
}
