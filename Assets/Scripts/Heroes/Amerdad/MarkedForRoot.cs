using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkedForRoot : Buff {

    public float damage;
    public  Buff rootBuff;

    private Gravity gravity;
    private PlayerControl playerControl;

    
    public override void BuffCharacter()
    {
        charStats.MarkedForRoot = true;
        gravity.GravityAction += Root;
    }

    public override void DebuffCharacter()
    {
        charStats.MarkedForRoot = false;
        gravity.GravityAction += Root;
    }

    private void Root()
    {
        playerControl.TakeAttack(damage, rootBuff.name);
    }

    void Start ()
    {
        gravity = GetComponent<Gravity>();
        playerControl = GetComponent<PlayerControl>();
	}
	

}
