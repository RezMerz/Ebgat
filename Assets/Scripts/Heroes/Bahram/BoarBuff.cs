using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarBuff : Buff {
    public override void BuffCharacter()
    {
        charStats.HeadState = EHeadState.Stunned;
    }

    public override void DebuffCharacter()
    {
        charStats.HeadState = EHeadState.Conscious;
    }
}
