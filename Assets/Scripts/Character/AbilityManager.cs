using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    private Ability ability1;
    private Ability ability2;
    private CharacterAttributes charStats;
	// Use this for initialization
	void Start () {
        charStats = GetComponent<CharacterAttributes>();
        Ability[] abilities = GetComponents<Ability>();
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i].abilityNumber == 1)
                ability1 = abilities[i];
            else if (abilities[i].abilityNumber == 2)
                ability2 = abilities[i];
        }
	}

    public void Ability1Pressed()
    {
        if(charStats.HeadState != EHeadState.Stunned)
            ability1.AbilityKeyPrssed();
    }

    public void Ability1Hold()
    {

    }

    public void Ability1Released()
    {

    }
    public void Ability2Pressed()
    {
        if (charStats.HeadState != EHeadState.Stunned)
            ability2.AbilityKeyPrssed();
    }

    public void Ability2Hold()
    {

    }

    public void Ability2Released()
    {

    }
}
