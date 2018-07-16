using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack {
    public GameObject bulletObj;

    private PlayerControl playerControl;

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
        GameObject cloneBulletObj = Instantiate(bulletObj);
        cloneBulletObj.GetComponent<Bullet>().Shoot(targetPos, transform.position);

    }
}
