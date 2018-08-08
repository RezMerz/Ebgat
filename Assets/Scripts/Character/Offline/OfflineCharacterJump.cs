using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineCharacterJump : MonoBehaviour {

    private CharacterAttributes charStats;

    private float jumpSpeed;
    private float gravitySpeed;
    private float actualSpeed;
    private List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();

    public float fullJumpHoldTime;

    private float timer;
    private bool jumpSpeedIncrease;
    private bool getCommand;

    private int mask;

    // Use this for initialization
    void Start()
    {
        mask = LayerMask.GetMask("Blocks");
        charStats = GetComponent<CharacterAttributes>();
        jumpSpeedIncrease = false;

    }
    private void Update()
    {
        if(charStats.FeetState == EFeetState.Jumping)
        {
            if(charStats.HeadState == EHeadState.Stunned)
            {
                charStats.FeetState = EFeetState.Falling;
            }
            else
            {
                Jumping();
            }
        }
    }

    // First Jump
    public void JumpPressed()
    {
        // Jump only if on 
        if (charStats.FeetState == EFeetState.Onground && charStats.HeadState !=EHeadState.Stunned )
        {
            getCommand = true;
            gravitySpeed = 0f;
            jumpSpeed = charStats.JumpSpeed;
            charStats.FeetState = EFeetState.Jumping;
        }

    }

    // Holding the Jump
    public void JumpHold()
    {
        if (getCommand && charStats.FeetState == EFeetState.Jumping)
        {
            jumpSpeed += charStats.JumpAcceleration * Time.deltaTime;
            if (jumpSpeed > charStats.JumpSpeedMax)
            {
                jumpSpeed = charStats.JumpSpeedMax;
            }
        }
    }

    public void JumpReleased()
    {
        getCommand = false;
    }

    private void Jumping()
    {
         actualSpeed = jumpSpeed - gravitySpeed;
         if (actualSpeed > 0)
         {
             bool hit = Toolkit.CheckMove(transform.position, charStats.size, Vector2.up, actualSpeed * Time.deltaTime,mask, out hitObjects);
             if (!hit)
             {
                 transform.position += Vector3.up * (actualSpeed * Time.deltaTime);
                 gravitySpeed += charStats.GravityAcceleration * Time.deltaTime;
             }
             else
             {
                 transform.position += Vector3.up * (hitObjects[0].distance);
                 charStats.FeetState = EFeetState.Falling;
             }
         }
         else
         {
             charStats.FeetState = EFeetState.Falling;
         }
    }
}
