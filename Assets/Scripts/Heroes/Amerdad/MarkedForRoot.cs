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
        gravity = charStats.GetComponent<Gravity>();
        playerControl = charStats.GetComponent<PlayerControl>();
        charStats.MarkedForRoot = true;
       // Debug.Log(gravity);
      //  Debug.Log(gravity.GravityAction);
        gravity.GravityAction += Root;
    }

    public override void DebuffCharacter()
    {
        charStats.MarkedForRoot = false;
        gravity.GravityAction -= Root;
    }

    private void Root()
    {
        playerControl.TakeAttack(playerControl,damage, rootBuff.name);
        charStats.MarkedForRoot = false;
    }

    public void Start ()
    {
        Debug.Log(charStats);
        Debug.Log(charStats.GetComponent<Gravity>());
	}
	

}
