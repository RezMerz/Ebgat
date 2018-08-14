using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack :Attack {
    public MeleeWaepon weapon;

    public override void AttackPressed()
    {
        if(cooldownTimer <= 0)
        {
            cooldownTimer = charStats.AttackCooldown;
            charStats.HandState = EHandState.Attacking;
            StartCoroutine(AttackAnimateTime());

        }
    }
}
