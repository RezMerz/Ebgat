using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack :Attack {
    public MeleeWaepon weapon;
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
        
        Vector2 direction = charStats.side;
        Vector2 origin = (Vector2)transform.position + (direction * (charStats.size / 2));
        weapon.Attack(origin, charStats.attackDamage, direction,256);
        
    }
}
