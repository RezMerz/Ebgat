using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousUlti : Ability {
    private bool abilityUse;
	// Use this for initialization
    public override void AbilityKeyPrssed()
    {
        if (!coolDownLock)
        {
            coolDownLock = true;
            abilityUse = true;
            charStats.AbilityState = EAbility.Ability1Start;
            StartCoroutine(CoolDownTimer(coolDownTime));
        }
    }

    void Update()
    {
        if (abilityUse)
        {
            
        }
    }

    public override void AbilityKeyHold()
    {

    }
    public override void AbilityKeyReleased()
    {

    }
    public override void AbilityActivateClientSide()
    {
        throw new System.NotImplementedException();
    }
}
