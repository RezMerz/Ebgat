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
         // Check the direction
        Vector2 dir = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 direction = Vector2.right;
        Vector2 origin = (Vector2)transform.position + (direction * (charStats.size / 2));
        print(origin);
        weapon.Attack(transform.position, charStats.attackDamage, Vector2.right, 256);
        
    }
}
