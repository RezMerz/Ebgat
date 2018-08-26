using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtashBahram : Ability {
    private BuffManager buffManager;

    void Start()
    {
        buffManager = GetComponent<BuffManager>();
    }
    public override void AbilityKeyPrssed()
    {
        if (!coolDownLock)
        {
            coolDownLock = true;
            StartCoroutine(CoolDownTimer(coolDownTime));
            buffManager.DebuffAllCharacter();
            charStats.AbilityState = EAbility.Ability2Start;
            buffManager.ActivateBuff(buff.name);
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
