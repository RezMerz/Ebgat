﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour {
    
    protected float cooldownTimer;
    protected CharacterAttributes charStats;

    protected PlayerControl playerControl;

	// Use this for initialization
    void Start () {
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        cooldownTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(cooldownTimer>0)
            cooldownTimer -= Time.deltaTime;
        
	}

    public virtual void AttackPressed(Vector2 mousePos) { }
}