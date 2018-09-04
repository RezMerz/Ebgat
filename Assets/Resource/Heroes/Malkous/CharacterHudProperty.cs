using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHudProperty : MonoBehaviour {


    public AbilityHudProperty[] abilities;
    private HUD hud;
    void Start()
    {
        Init();
    }

    public void Init()
    {
        if (GetComponent<PlayerControlClientside>().IsLocalPlayer())
        {
            hud = GameObject.FindObjectOfType<HUD>();
            for (int i = 0; i < abilities.Length; i++)
            {
                print(abilities[i].icon);
                hud.SetImage(abilities[i].icon, i + 1);
            }
        }
    }
}


[System.Serializable]
public class AbilityHudProperty
{


    public Sprite icon;
    public float cooldown;
    public float timer;



}
