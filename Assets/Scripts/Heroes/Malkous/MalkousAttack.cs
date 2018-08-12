using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousAttack : Attack {
    public float baseDamage;
    public float maxDamage;
    private float damage;
    private bool attackCharge;
    public float damageMultiplier;
    public Bullet bulletPrefab;
    private CharacterAttributes charStats;
    public override void AttackPressed() {

        if (cooldownTimer <= 0)
        {
            print("Attack Malkous Attack");
            // Graphic Code that attack started
            damage = baseDamage;
            attackCharge = true;
            cooldownTimer = charStats.AttackCooldown;
            charStats.HandState = EHandState.AttackCharge;
        }

        
    }

    public override void AttackHold() {}


    public override void AttackReleased() {
        if (attackCharge)
        {
            charStats.HandState = EHandState.Attacking;
            attackCharge = false;
            cooldownTimer = charStats.AttackCooldown;
            // Calculate Side
            Vector2 attackSide = charStats.AimSide;
            if (attackSide.y == 0 && attackSide.x == 0)
                attackSide = charStats.Side;

            print("Bullet:" + attackSide);

            //playerControl.serverNetworkSender.ClientRangedAttack(playerControl.clientNetworkSender.PlayerID, attackSide);
           // Bullet bullet =  Instantiate(bulletPrefab);
        }
    }

	// Use this for initialization
	void Start () {
		charStats = GetComponent<CharacterAttributes>();
	}
	
	// Update is called once per frame
	void Update () {
		if(attackCharge)
        {

            if (damage + Mathf.Floor(Time.deltaTime * 100) * damageMultiplier <= maxDamage)
                damage += Mathf.Floor(Time.deltaTime * 100) * damageMultiplier;
            else
                damage = maxDamage;
        }
        else
        {
            if (cooldownTimer > 0)
                cooldownTimer -= Time.deltaTime;
        }
	}
}
