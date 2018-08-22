using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraArmor : Ability{
    public Buff buff;
    public int radius;
    private bool abilityUse;

    public override void AbilityKeyPrssed()
    {
        if(!coolDownLock)
        {
            coolDownLock = true;
            abilityUse = true;
            StartCoroutine(CoolDownTimer(coolDownTime));
        }
    }

    void Update()
    {
        if (abilityUse)
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask(charStats.teamName));
            foreach(Collider2D obj in hitObjects)
            {
                obj.GetComponent<PlayerControl>().TakeAttack(0, buff.name);
            }
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
