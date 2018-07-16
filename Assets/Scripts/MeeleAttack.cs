using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleAttack :Attack {


    public override void AttackPressed(Vector2 mousePos)
    {
        // Attack Cooldown
        if (cooldownTimer <= 0)
        {
            cooldownTimer = charStats.attackCooldown;
            Attack(mousePos);
        }
    }

    private void Attack(Vector2 mousePos)
    {

    }
}
