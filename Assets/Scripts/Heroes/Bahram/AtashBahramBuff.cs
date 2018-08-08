using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtashBahramBuff : Buff {
    public float deltaDamage;

    public override void BuffCharacter()
    {
        charStats.AttackDamage += deltaDamage;
    }

    public override void DebuffCharacter()
    {
        charStats.AttackDamage -= deltaDamage;
    }
}
