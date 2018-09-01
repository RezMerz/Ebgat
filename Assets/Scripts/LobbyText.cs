using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyText : MonoBehaviour {

    public Text[] texts;


    public void OnChangeTeamClick(){
        GameManager.instance.ChangeTeamclicked();
    }
}
