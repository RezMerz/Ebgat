﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {
    protected float cooldownTimer;
    protected CharacterAttributes charStats;

	// Use this for initialization
	void Start () {
        charStats = GetComponent<CharacterAttributes>();
        cooldownTimer = 0;
        if (charStats.attackMode == EAttackMode.Ranged)
            if (!(this is RangedAttack))
                print("Character Attack Mode is Range but Component is not");
          
        if (charStats.attackMode == EAttackMode.Melee)
            if (!(this is MeleeAttack))
                print("Character Attack Mode is Melee but Component is not");
	}
	
	// Update is called once per frame
	void Update () {
        if(cooldownTimer>0)
            cooldownTimer -= Time.deltaTime;
        
	}

    public virtual void AttackPressed(Vector2 mousePos) { }
}
