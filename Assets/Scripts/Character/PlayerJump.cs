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
    private bool jumpSpeedIncrease;

	// Use this for initialization
	void Start ()
    {
        charStats = GetComponent<CharacterAttributes>();
        jumpSpeedIncrease = false;

	}
    private void Update()
    {
        Jumping();
    }

    // First Jump
    public void JumpPressed()
    {
        // Jump only if on 
        if (charStats.FeetState == EFeetState.Onground)
        {
            jumpSpeedIncrease = false;
            timer = 0;
            gravitySpeed = 0f;
            jumpSpeed = charStats.jumpSpeed;
            charStats.FeetState = EFeetState.Jumping;
        }
        
    }

    // Holding the Jump
    public void JumpHold()
    {
        if (charStats.FeetState == EFeetState.Jumping)
        {
            timer += Time.deltaTime;
            if (timer >= fullJumpHoldTime)
            {
                IncreaseJumpSpeed();
            }
        }
    }

    public void IncreaseJumpSpeed(){
        jumpSpeedIncrease = true;
    }

    public void JumpReleased()
    {
        timer = -Mathf.Infinity;
    }

    private void Jumping()
    {
        if(charStats.FeetState == EFeetState.Jumping)
        {
            if (jumpSpeedIncrease)
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
                    charStats.FeetState = EFeetState.Falling;
                }

            }
            else
            {
                charStats.FeetState = EFeetState.Falling;
            }
        }
    }
}
