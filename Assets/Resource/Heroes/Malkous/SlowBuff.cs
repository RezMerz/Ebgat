using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowBuff : Buff {
    public float slowRate;
    



    public override void BuffCharacter()
    {
        charStats.SpeedRate = charStats.SpeedRate * slowRate;
    }

    public override void DebuffCharacter()
    {
        charStats.SpeedRate = charStats.SpeedRate / slowRate;
    }
}
