using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousAttack : Attack
{
    public VirtualBullet virttualBullet;
    public float maxHoldTime;
    public float damageMultiplier;

    private bool attackCharge;
    private float timer;
    private int layerMask;
    private int playerID;

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
        if(charStats.HeadState != EHeadState.Stunned)
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
        float range = charStats.Range;
        int bulletID = ServerManager.instance.GetBulletID(playerControl.playerId);
        // Calculate Side
        Vector2 attackSide = charStats.AimSide;
        if (attackSide == Vector2.zero)
            attackSide = charStats.Side;
        VirtualBullet virtualBullet = Instantiate(virttualBullet,transform.position + (Vector3)charStats.Side * 2 + Vector3.up * 0.5f,Quaternion.identity);
        virtualBullet.transform.position = transform.position;
        virtualBullet.Shoot(damage, attackSide, layerMask, gravityAcc,playerControl,bulletID);
        // register bullet
        playerControl.worldState.BulletRegister(playerControl.playerId, bulletID, attackSide, gravityAcc);
    }
}
