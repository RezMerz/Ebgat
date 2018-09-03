using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    public Buff[] buffListRefrence;


    private CharacterAttributes charStats;
    private List<Buff> activatedBuffList;

    void Start()
    {
        charStats = GetComponent<CharacterAttributes>();
        activatedBuffList = new List<Buff>();
    }
    public void ActivateBuff(string name)
    {
        foreach (Buff buff in buffListRefrence)
            if (buff.name.Equals(name))
            {
                if (buff.stackable)
                {
                    AddBuffToList(buff);
                }
                else
                {
                    bool found = false;
                    foreach (Buff thisBuff in activatedBuffList)
                    {
                        if (thisBuff.name == buff.name)
                        {
                            // There is already a buff with this name, just extend the time
                            thisBuff.DurationReset();
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        // There isn't a buff with this name so create one
                        AddBuffToList(buff);
                    }
                }

            }
    }
    // Instantiate and add Buff to list of activaded buffs
    private void AddBuffToList(Buff buff)
    {
        Buff thisBuff = Instantiate(buff);
        thisBuff.buffManager = this;
        thisBuff.charStats = charStats;
        thisBuff.StartBuff();
        activatedBuffList.Add(thisBuff);
    }

    public void DebuffAllCharacter()
    {
        while(activatedBuffList.Count > 0)
        {
            activatedBuffList[0].FinishBuff();
        }
    }

    public void RemoveBuffFromList(Buff buff)
    {
        activatedBuffList.Remove(buff);
    }
}
