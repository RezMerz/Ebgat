using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarBuff : Buff {
    public override void BuffCharacter()
    {
        charStats.HeadState = EHeadState.Stunned;
        if(charStats.HandState == EHandState.Attacking)
        {
            GetComponent<Attack>().IntruptAttack();
        }
        if(charStats.HandState == EHandState.Casting)
        {
            Ability[] abilities = GetComponents<Ability>();
            foreach (Ability ability in abilities)
            {
                ability.IntruptCast();
            }
        }
    }

    public override void DebuffCharacter()
    {
        charStats.HeadState = EHeadState.Conscious;
    }
}
