using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousAttack : Attack {
    public float baseDamage;
    public float maxDamage;
    private float damage;
    private bool attackCharge;
    public override void AttackPressed(Vector2 attackDir) { 
        // Graphic Code that attack started
        damage = baseDamage;
        attackCharge = true;

        
    }

    public override void AttackHold() { 
        
    }


    public override void AttackReleased() { 
      
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(attackCharge)
        {
            if (damage <= maxDamage)
                damage += Time.deltaTime;
            else
                damage = maxDamage;
        }
	}
}
