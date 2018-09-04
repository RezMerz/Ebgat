using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorBuff : Buff {
    public float deltaArmor;
    public override void BuffCharacter()
    {
        charStats.Armor += deltaArmor;
    }

    public override void DebuffCharacter()
    {
        charStats.Armor -= deltaArmor;
    }
}
