using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{

    private PlayerControl playerControl;
    private CharacterAttributes charStats;
    private CharacterPhysic physic;

    private bool isHolding;
    private bool doubleJumped;

    private bool jumped;


    // Use this for initialization
    void Start()
    {
        charStats = GetComponent<CharacterAttributes>();
        playerControl = GetComponent<PlayerControl>();
        physic = GetComponent<CharacterPhysic>();
    }
    private void FixedUpdate()
    {
        if (playerControl.IsServer())
        {
            if (jumped)
            {
                physic.PhysicAction += HitFunction;
                JumpServerside();
                //if (charStats.HeadState == EHeadState.Stunned)
                //{
                //    // jumped = false;
                //    //charStats.FeetState = EFeetState.Falling;
                //    // charStats.ResetGravitySpeed();
                //}
                //else
                //{
                //}
            }
        }
    }

    // First Jump
    public void JumpPressed()
    {
        // Jump only if on 
        if (charStats.FeetState == EFeetState.Onground && charStats.HeadState != EHeadState.Stunned)
        {
            if (charStats.Energy >= charStats.jumpEnergyConsume)
            {
                jumped = true;
                isHolding = true;
                JumpServerside();
                doubleJumped = false;
            }
            else
            {
                print("Low Energy");
            }
        }
        // Double Jump
        else if (charStats.canDoubleJump && !doubleJumped && (charStats.FeetState == EFeetState.Falling || charStats.FeetState == EFeetState.Jumping) && charStats.HeadState != EHeadState.Stunned)
        {
            doubleJumped = true;
            jumped = true;
            charStats.ResetJumpSpeed();
            charStats.JumpSpeedMax += charStats.GravitySpeed;
            charStats.JumpSpeed += charStats.GravitySpeed;
            physic.RemoveTaggedForces(0);
            //charStats.ResetGravitySpeed();
            charStats.FeetState = EFeetState.DoubleJumping;
        }
    }
    // Holding the Jump
    public void JumpHold()
    {
        charStats.JumpSpeed += charStats.JumpAcceleration * Time.deltaTime;
        if (charStats.JumpSpeed > charStats.JumpSpeedMax)
        {
            charStats.JumpSpeed = charStats.JumpSpeedMax;
        }
    }
    public void JumpReleased()
    {
        isHolding = false;
    }
    private void JumpServerside()
    {
        if (isHolding || charStats.FeetState == EFeetState.DoubleJumping)
        {
            JumpHold();
        }
        Vector2 force = Vector2.up * (charStats.JumpSpeed * Time.deltaTime);
        physic.AddForce(force);
        physic.PhysicAction += HitFunction;
    }
    private void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        if (vHits.Count > 0)
        {

            jumped = false;
            if(direction.y <0)
            {
                doubleJumped = false;
            }
           // charStats.ResetJumpSpeed();
            
        }
    }

}

