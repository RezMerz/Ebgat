using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootBuff : Buff {

    public override void BuffCharacter()
    {
        charStats.Root = true;
    }

    public override void DebuffCharacter()
    {
        charStats.Root =  false;
    }
}
