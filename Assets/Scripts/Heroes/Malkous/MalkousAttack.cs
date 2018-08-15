using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousAttack : Attack
{
    public Bullet bulletPrefab;
    public Bullet virttualBullet;
    public float maxHoldTime;
    public float damageMultiplier;
    public float range;

    private bool attackCharge;
    private float timer;
    private int layerMask;

    private new void Start()
    {
        charStats = GetComponent<CharacterAttributes>();
        playerControl = GetComponent<PlayerControl>();
        layerMask = LayerMask.GetMask("Blocks", charStats.enemyTeamName);
    }
    void FixedUpdate()
    {
        if (attackCharge)
        {
            timer += Time.deltaTime;
            if (timer > maxHoldTime)
            {
                timer = maxHoldTime;
            }
        }
        else
        {
            if (cooldownTimer > 0)
                cooldownTimer -= Time.deltaTime;
        }
    }
    public override void AttackPressed()
    {
        if (cooldownTimer <= 0)
        {
            timer = 0;
            attackCharge = true;
            cooldownTimer = charStats.AttackCooldown;
            charStats.HandState = EHandState.AttackCharge;
        }
    }

    public override void AttackHold() { }

    public override void AttackReleased()
    {
        if (attackCharge)
        {
            attackCharge = false;
            charStats.HandState = EHandState.Attacking;
            cooldownTimer = charStats.AttackCooldown;
            StartCoroutine(AttackAnimateTime());
        }
    }

    protected override void ApplyAttack()
    {
        float damage = charStats.AttackDamage * (1 + (timer * damageMultiplier));
        float gravityAcc = charStats.GravityAcceleration * (timer / maxHoldTime); 
        // Calculate Side
        Vector2 attackSide = charStats.AimSide;
        if (attackSide == Vector2.zero)
            attackSide = charStats.Side;
        Bullet bullet = Instantiate(virttualBullet);
        bullet.transform.position = transform.position;
        bullet.Shoot(damage, attackSide, playerControl, layerMask, gravityAcc, range);
        // register bullet
    }
}
