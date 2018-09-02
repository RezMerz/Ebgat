using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Ability {
    private BuffManager buffManager;

    public float time;
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
            StartCoroutine(AbilityTime());
            buffManager.DebuffAllCharacter();
            charStats.AbilityState = EAbility.Ability2Start;
            //buffManager.ActivateBuff(buff.name);
        }
    }

    private IEnumerator AbilityTime()
    {
        yield return new WaitForSeconds(time);
        charStats.AbilityState = EAbility.Ability2Finish;
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
