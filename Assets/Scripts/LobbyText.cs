using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyText : MonoBehaviour {
    public GameObject[] Heroes;
    private int counter = 0;
    public Text[] texts;
    private HeroSelect heroSelect;

    void Start()
    {
        heroSelect = GameObject.FindObjectOfType<HeroSelect>();
    }

    public void HeroRight()
    {
        if (counter == 2)
            counter = 0;
        else
            counter++;
        heroSelect.HeroShow(counter);
        LoadHeroInfo(counter);
    }

    public void HeroLeft()
    {

        if (counter == 0)
            counter = 2;
        else
            counter--;

       heroSelect.HeroShow(counter);
       LoadHeroInfo(counter);
    }

    private void LoadHeroInfo(int value)
    {

    }

    public void OnChangeTeamClick(){
        GameManager.instance.OnChangeTeamClicked();
    }

    public void OnStartClicked(){
        GameManager.instance.OnStartClicked();
    }

    public void OnLeaveClicked(){
        GameManager.instance.OnLeaveClicked();
    }


}
