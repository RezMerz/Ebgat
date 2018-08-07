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
        playerControl = GetComponent<PlayerControl>();
        charstats = GetComponent<CharacterAttributes>();
        layerMask = LayerMask.GetMask("Blocks", charstats.enemyTeamName);
        gravityLayerMask = LayerMask.GetMask("Blocks","Bridge", charstats.enemyTeamName);
    }
	
    protected override void HitFunction(List<RaycastHit2D> hits, Vector2 direction)
    {
            
    }
}
