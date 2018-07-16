using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {
    CharacterAttributes charStats;

    private float jumpSpeed;
    private List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
	// Use this for initialization
	void Start ()
    {
        charStats = GetComponent<CharacterAttributes>();
	}
    private void Update()
    {
        Jumping();
    }

    public void JumpPressed()
    {
        // Jump only if on ground
        if (charStats.FeetState == EFeetState.Onground)
        {
            charStats.FeetState = EFeetState.Jumping;
            jumpSpeed = charStats.jumpSpeed;
        }
    }

    private void Jumping()
    {
        if(charStats.FeetState == EFeetState.Jumping)
        {
            if(jumpSpeed > 0)
            {
                bool hit = Toolkit.CheckMove(transform.position, charStats.size, Vector2.up, jumpSpeed*Time.deltaTime, 256, out hitObjects);
                if (!hit)
                {
                    transform.position += Vector3.up * (jumpSpeed * Time.deltaTime);
                    jumpSpeed -= charStats.gravityAcceleration;
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
