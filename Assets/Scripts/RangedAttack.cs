using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack {

    public override void AttackPressed(Vector2 mousePos)
    {
        // Attack Cooldown
        if (cooldownTimer <= 0)
        {
            cooldownTimer = charStats.attackCooldown;
            Attack(mousePos);
        }
    }

    public void Attack(Vector2 mousePos)
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        int layer = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
        playerControl.CmdShootbullet(targetPos, transform.position, charStats.attackDamage,layer);
    }
}
