using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtashBahram : Ability {
    private BuffManager buffManager;
    public override void AbilityKeyPrssedServerSide()
    {
        if (!coolDownLock)
        {
            coolDownLock = true;
            StartCoroutine(CoolDownTimer(coolDownTime));
            buffManager.DebuffCharacter();
            buffManager.ActivateBuff("AtashBahramBuff");
        }
    }
    public override void AbilityKeyHold()
    {
        throw new System.NotImplementedException();
    }



    public override void AbilityKeyReleased()
    {
        throw new System.NotImplementedException();
    }
}
