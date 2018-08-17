using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    public MeleeWaepon weapon;
    public float attackForce;
    private Vector2 weaponSize;
    private float distance;
    private int layerMask;
    private string buffName;

    private new void Start()
    {
        base.Start();
        weaponSize = new Vector2(transform.localScale.x * GetComponent<BoxCollider2D>().size.x * 0.5f, weapon.size.y);
        distance = weapon.size.x;
        layerMask = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
        if (weapon.buff != null)
        {
            buffName = weapon.buff.name;
        }
        else
        {
            buffName = "";
        }
    }

    public override void AttackPressed()
    {
        if (cooldownTimer <= 0)
        {
            cooldownTimer = charStats.AttackCooldown;
            charStats.HandState = EHandState.Attacking;
            StartCoroutine(AttackAnimateTime());
        }
    }
    protected override void ApplyAttack()
    {
        RaycastHit2D[] targets = Physics2D.BoxCastAll(transform.position, weaponSize, 0, charStats.Side, distance, layerMask, 0, 0);
        foreach (RaycastHit2D target in targets)
        {
            if (target.collider.tag.Equals("Player"))
            {
                target.collider.gameObject.GetComponent<PlayerControl>().TakeAttack(charStats.AttackDamage, buffName);
                target.collider.gameObject.GetComponent<CharacterPhysic>().AddForce(charStats.Side * attackForce);
            }
            else
            {
                playerControl.physic.AddForce(charStats.Side * -attackForce);
            }
        }
    }
}
