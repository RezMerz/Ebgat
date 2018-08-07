﻿using System;
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
	
    public void IncludeBridge()
    {
        gravityLayerMask = LayerMask.GetMask("Blocks", "Bridge",charstats.enemyTeamName);
        charstats.ResetCayoteTime();
    }
    public void ExcludeBridge()
    {
        gravityLayerMask = layerMask;
        charstats.cayoteTime = 0;
    }
    protected override void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {

            
    }
}
