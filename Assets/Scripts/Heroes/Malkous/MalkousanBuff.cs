using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousanBuff : Buff {

    public int deltaMoveSpeed;
    public override void BuffCharacter()
    {
        charStats.MoveSpeed += deltaMoveSpeed;
    }

    public override void DebuffCharacter()
    {
        charStats.MoveSpeed -= deltaMoveSpeed;
    }
}
