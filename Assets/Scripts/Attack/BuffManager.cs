using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour {
    private CharacterAttributes charStats;
    public Buff[] buffListRefrence;

    void Start()
    {
        charStats = GetComponent<CharacterAttributes>();
    }
    public void ActivateBuff(string name)
    {
        foreach(Buff buff in buffListRefrence)
            if (buff.name == name)
            {
                Buff thisBuff = Instantiate(buff);
                thisBuff.charStats = charStats;
            }
    }
}
