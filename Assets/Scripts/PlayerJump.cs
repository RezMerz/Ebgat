using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {
    CharacterAttributes charStats;

    private float jumpSpeed;
    private float gravitySpeed;
    private float actualSpeed;
    private List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();

    public float fullJumpHoldTime;

    private float timer;
    private bool jumpHold;
    private bool jumpCommand;
    private bool jumpSpeedINcrease;

	// Use this for initialization
	void Start ()
    {
        charStats = GetComponent<CharacterAttributes>();
        jumpHold = true;
        jumpCommand = true;
        jumpSpeedINcrease = false;

	}
    private void Update()
    {
        Jumping();
    }

    public void JumpPressed()
    {
        if ( jumpHold && charStats.FeetState == EFeetState.Jumping)
        {
            timer += Time.deltaTime;
            if(timer >= fullJumpHoldTime)
            {
                jumpSpeedINcrease = true;
                jumpHold = false;
            }
        }
        // Jump only if on 
        if (jumpCommand && charStats.FeetState == EFeetState.Onground)
        {
            jumpCommand = false;
            jumpSpeedINcrease = false;
            timer = 0;
            gravitySpeed = 0f;
            jumpSpeed = charStats.jumpSpeed;
            charStats.FeetState = EFeetState.Jumping;
        }
        
    }
    public void JumpReleased()
    {
        if(charStats.FeetState == EFeetState.Jumping)
        {
            jumpHold = false;
        }
        jumpCommand = true;
    }

    private void Jumping()
    {
        if(charStats.FeetState == EFeetState.Jumping)
        {
            if (jumpSpeedINcrease)
            {
                jumpSpeed += charStats.jumpAcceleration;
                if(jumpSpeed > charStats.jumpSpeedMax)
                {
                    jumpSpeed = charStats.jumpSpeedMax;
                }
            }
            actualSpeed = jumpSpeed - gravitySpeed;
            if(actualSpeed > 0)
            {
                bool hit = Toolkit.CheckMove(transform.position, charStats.size, Vector2.up, actualSpeed*Time.deltaTime, 256, out hitObjects);
                if (!hit)
                {
                    transform.position += Vector3.up * (actualSpeed * Time.deltaTime);
                    gravitySpeed += charStats.gravityAcceleration;
                }
                else
                {
                    transform.position += Vector3.up * (hitObjects[0].distance);
                    jumpHold = true;
                    charStats.FeetState = EFeetState.Falling;
                }

            }
            else
            {
                jumpHold = true;
                charStats.FeetState = EFeetState.Falling;
            }
        }
    }
}
