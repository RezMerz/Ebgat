using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {
    PlayerControl playerControl;
    CharacterAttributes charStats;

    private float jumpSpeed;
    private float gravitySpeed;
    private float actualSpeed;

    private List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();

    private bool isHolding;

    private int layerMask;

    // Use this for initialization
    void Start()
    {
        charStats = GetComponent<CharacterAttributes>();
        playerControl = GetComponent<PlayerControl>();
        layerMask = LayerMask.GetMask("Blocks", charStats.enemyTeamName);
    }
    private void Update()
    {
        if (playerControl.IsServer())
        {
            if (charStats.FeetState == EFeetState.Jumping)
            {
                if (charStats.HeadState == EHeadState.Stunned)
                {
                    charStats.FeetState = EFeetState.Falling;
                    ///change state of client
                }
                else
                {
                    JumpServerside();
                }
            }
        }
        else
        {

        }
    }

    // First Jump
    public void JumpPressed()
    {
        // Jump only if on 
        if (charStats.FeetState == EFeetState.Onground && charStats.HeadState != EHeadState.Stunned)
        {
            gravitySpeed = 0f;
            jumpSpeed = charStats.jumpSpeed;
            charStats.FeetState = EFeetState.Jumping;
        }
    }

    // Holding the Jump
    public void JumpHold()
    {
         jumpSpeed += charStats.jumpAcceleration * Time.deltaTime;
         if (jumpSpeed > charStats.jumpSpeedMax)
         {
             jumpSpeed = charStats.jumpSpeedMax;
         }
    }

    public void JumpReleased()
    {

    }

    private void Jumping()
    {
        actualSpeed = jumpSpeed - gravitySpeed;
        if (actualSpeed > 0)
        {
            bool hit = Toolkit.CheckMove(transform.position, charStats.size, Vector2.up, actualSpeed * Time.deltaTime, layerMask, out hitObjects);
            if (!hit)
            {
                transform.position += Vector3.up * (actualSpeed * Time.deltaTime);
                gravitySpeed += charStats.gravityAcceleration * Time.deltaTime;
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

    public void JumpServerside()
    {
        if (isHolding)
        {
            JumpHold();
        }
        actualSpeed = jumpSpeed - gravitySpeed;
        if (actualSpeed > 0)
        {
            float des;
            bool hit = Toolkit.CheckMove(transform.position, charStats.size, Vector2.up, actualSpeed * Time.deltaTime, layerMask, out hitObjects);
            if (!hit)
            {
                
                transform.position += Vector3.up * (actualSpeed * Time.deltaTime);
                gravitySpeed += charStats.gravityAcceleration * Time.deltaTime;

                des = transform.position.y;
            }
            else
            {
                transform.position += Vector3.up * (hitObjects[0].distance);
                charStats.FeetState = EFeetState.Falling;

                des = transform.position.y;
            }
        }
        else
        {
            charStats.FeetState = EFeetState.Falling;
        }

    }

}

