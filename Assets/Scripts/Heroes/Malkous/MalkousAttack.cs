using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousAttack : Attack
{
    public GameObject virttualBullet;
    public float maxHoldTime;
    public float damageMultiplier;
    public float minRange;
    public float maxRange;

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
            if (charStats.Energy >= charStats.attackEnergyConsume)
            {
                timer = 0;
                charStats.HandState = EHandState.Attacking;
                StartCoroutine(AttackAnimateTime());
                cooldownTimer = charStats.AttackCooldown;
                cooldownTimer = charStats.AttackCooldown;
            }
            else
            {
                print(" Low Energy");
            }
        }
    }

    public override void AttackHold() { }

    public override void AttackReleased()
    {

    }

    protected override void ApplyAttack()
    {
        float damage = charStats.AttackDamage * (1 + (timer * damageMultiplier));
        float gravityAcc = charStats.GravityAcceleration;
        float range = maxRange - (maxRange - minRange) * (timer / maxHoldTime); 
        int bulletID = ServerManager.instance.GetBulletID(playerControl.playerId);
        // Calculate Side
        Vector2 attackSide = charStats.AimSide;
        print(charStats.AimSide);
        if (attackSide == new Vector2(0,0))
            attackSide = charStats.Side;
        GameObject virtualBullet = Instantiate(virttualBullet,transform.position + (Vector3)charStats.Side * 2 + Vector3.up * 0.5f,Quaternion.identity);
        virtualBullet.layer = gameObject.layer;
        virtualBullet.GetComponent<VirtualBullet>().Shoot(damage, attackSide, layerMask, gravityAcc,playerControl,bulletID,range);
        // register bullet
        playerControl.worldState.BulletRegister(playerControl.playerId, bulletID, attackSide, gravityAcc,range);
    }
}
