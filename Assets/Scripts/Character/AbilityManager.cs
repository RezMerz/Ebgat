using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour {
    private Ability ability1;
    private Ability ability2;
	// Use this for initialization
	void Start () {
       if (GetComponents<Ability>().Length>=1)
            ability1 =  GetComponents<Ability>()[0];
       if (GetComponents<Ability>().Length >= 2)
            ability2 = GetComponents<Ability>()[1];
	}

    public void Ability1Pressed()
    {
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

    }

    public void Ability2Hold()
    {

    }

    public void Ability2Released()
    {

    }
}
