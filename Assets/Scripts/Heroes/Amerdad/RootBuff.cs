using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBuff : Buff {

    public override void BuffCharacter()
    {
        charStats.Root = true;
        charStats.GetComponent<Physic>().Lock();
    }

    public override void DebuffCharacter()
    {
        charStats.Root =  false;
        charStats.GetComponent<Physic>().Unlock();
    }
}
