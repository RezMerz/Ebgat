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
       if (GetComponents<Ability>().Length>=1)
            ability1 =  GetComponents<Ability>()[0];
       if (GetComponents<Ability>().Length >= 2)
            ability2 = GetComponents<Ability>()[1];
	}

    public void Ability1Pressed()
    {
        if(charStats.HeadState != EHeadState.Stunned)
            ability1.AbilityKeyPrssedServerSide();
    }

    public void Ability1Hold()
    {

    }

    public void Ability1Released()
    {

    }
    public void Ability2Pressed()
    {

    }

    public void Ability2Hold()
    {

    }

    public void Ability2Released()
    {

    }
}
