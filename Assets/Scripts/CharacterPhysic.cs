using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysic : Physic {
    private CharacterAttributes charstats;
    private Gravity gravity;


	// Use this for initialization
	void Start ()
    {
        charstats = GetComponent<CharacterAttributes>();	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    	
	}

    public override void HitFunction(List<RaycastHit2D> hits, Vector2 direction)
    {
        if(direction == Vector2.up)
        {
            if(charstats.FeetState == EFeetState.Jumping)
            {
                charstats.FeetState = EFeetState.Falling;
                
            }
        }
        else if(direction == Vector2.down)
        {

        }
        else
        {

        }
    }
}
