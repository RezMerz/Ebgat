using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBuff : Buff {

    public override void BuffCharacter()
    {
        charStats.FeetState = EFeetState.Root;
    }

    public override void DebuffCharacter()
    {
        charStats.FeetState = EFeetState.Onground;
    }
}
