using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousAttack : Attack {
    public float baseDamage;
    public float maxDamage;
    private float damage;
    private bool attackCharge;
    public float damageMultiplier;
    private CharacterAttributes charStats;
    public override void AttackPressed() { 
        // Graphic Code that attack started
        damage = baseDamage;
        attackCharge = true;

        
    }

    public override void AttackHold() { 
        
    }


    public override void AttackReleased() {
        // Bullet Code Here
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
	}
}
