using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtashBahram : Ability {
    private BuffManager buffManager;
    public override void AbilityKeyPrssed()
    {
        if (!coolDownLock)
        {
            coolDownLock = true;
            StartCoroutine(CoolDownTimer(coolDownTime));
            buffManager.DebuffAllCharacter();
            buffManager.ActivateBuff("AtashBahramBuff");
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
