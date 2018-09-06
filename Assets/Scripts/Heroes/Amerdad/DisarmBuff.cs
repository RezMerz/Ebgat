using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisarmBuff : Buff {

    public override void BuffCharacter()
    {
        charStats.Disarm = true;
        if(charStats.HandState == EHandState.Attacking)
        {
            GetComponent<Attack>().IntruptAttack();
        }
    }

    public override void DebuffCharacter()
    {
        charStats.Disarm = false;
    }
}
