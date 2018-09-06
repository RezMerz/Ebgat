using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gravity : MonoBehaviour {
    public Action GravityAction;


    private PlayerControl playerControl;
    private CharacterPhysic physic;
    private CharacterAttributes charStats;

    private float timer;

    private bool start;

   
    // Use this for initialization
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerControl.ReadyAction += Initialize;
    }

    private void Initialize()
    {
        physic = GetComponent<CharacterPhysic>();
        charStats = GetComponent<CharacterAttributes>();
        start = true;
    }



    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerControl.IsServer() && start)
        {
            GravityServerside();
        }
    }
    private void GravityServerside()
    {
        if (charStats.FeetState != EFeetState.NoGravity)
        {
            SpeedCheck();
            Vector2 force = Vector2.down * (charStats.GravitySpeed * Time.deltaTime);
            physic.AddForce(force);
            physic.PhysicAction += HitFunction;
        }
    }
    private void SpeedCheck()
    {
        if(charStats.FeetState == EFeetState.Onground)
        {
            charStats.ResetGravitySpeed();
        }
        else if (charStats.FeetState == EFeetState.OnWall)
        {
            physic.RemoveTaggedForces(0);
            charStats.GravityOnWall();
        }
        else
        {
            charStats.GravitySpeed += charStats.GravityAcceleration * Time.deltaTime;
        }
    }

    private void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        if (vHits.Count > 0 && direction.y <= 0)
        {
            timer = 0;
            charStats.FeetState = EFeetState.Onground;
            if(GravityAction != null)
            {
                GravityAction();
                GravityAction = null;
            }
        }
        else
        {
            if(charStats.FeetState == EFeetState.Onground)
            {
                timer += Time.deltaTime;
                if(timer >= charStats.CayoteTime)
                {
                    charStats.FeetState = EFeetState.Falling;
                }
            }
        }
    }


}                   
