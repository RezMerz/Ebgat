using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHudProperty : MonoBehaviour {


    public AbilityHudProperty[] abilities;
   
}


[System.Serializable]
public class AbilityHudProperty
{
    public Sprite icon;
    public float cooldown;
    public float timer;
}
