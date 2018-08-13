using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {

    private PlayerControl playerControl;
    private CharacterAttributes charStats;
    private CharacterPhysic physic;

    private bool isHolding;


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
            if (charStats.FeetState == EFeetState.Jumping || charStats.FeetState == EFeetState.DoubleJumping)
            {
                if (charStats.HeadState == EHeadState.Stunned)
                {
                    charStats.FeetState = EFeetState.Falling;
                    charStats.ResetGravitySpeed();
                }
                else
                {
                    JumpServerside();
                }
            }
        }
    }

    // First Jump
    public void JumpPressed()
    {
        // Jump only if on 
        if (charStats.FeetState == EFeetState.Onground && charStats.HeadState != EHeadState.Stunned)
        {
            charStats.ResetJumpSpeed();
            charStats.FeetState = EFeetState.Jumping;
            isHolding = true;
            JumpServerside();
        }
            // Double Jump
        else if (charStats.FeetState == EFeetState.Jumping && charStats.canDoubleJump) 
        {
            charStats.ResetJumpSpeed();
            charStats.ResetGravitySpeed();
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
    private  void JumpServerside()
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
        if(vHits.Count > 0)
        {
            if (direction.y > 0)
            {
                charStats.FeetState = EFeetState.Falling;
                charStats.ResetGravitySpeed();
            }
        }
    } 

}

