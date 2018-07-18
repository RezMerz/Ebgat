using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {


    private CharacterAttributes charStats;
    private List<RaycastHit2D> hitObjects;
    private Transform transformHero;
    private float timer;

   
    // Use this for initialization
    void Start()
    {
        transformHero = transform;
        hitObjects = new List<RaycastHit2D>();
        charStats = GetComponent<CharacterAttributes>();
    }

    // Update is called once per frame
    void Update()
    {
        SpeedCheck();
        HeroGravity();
    }

    private void HeroGravity()
    {
        // not int jumping state
        if (charStats.FeetState != EFeetState.Jumping)
        {
            bool hit = Toolkit.CheckMove(transformHero.position, charStats.size, Vector2.down, Time.deltaTime * charStats.gravitySpeed, 256, out hitObjects);

            if (charStats.FeetState == EFeetState.Onground)
            {
                // Go to Falling
                if(!hit)
                {
                    if(timer > charStats.cayoteTime)
                    {
                        charStats.FeetState = EFeetState.Falling;
                        transformHero.position += Vector3.down * (Time.deltaTime * charStats.gravitySpeed);
                        timer = 0;
                    }
                    else
                    {
                        timer += Time.deltaTime;
                    }
                }

            }
            else if (charStats.FeetState == EFeetState.Falling)
            {
                // Go to on Ground state
                if(hit)
                {
                    transformHero.position += Vector3.down * (hitObjects[0].distance);
                    charStats.FeetState = EFeetState.Onground;
                }
                // Still Faliing
                else
                {
                    transformHero.position += Vector3.down * (Time.deltaTime * charStats.gravitySpeed);
                }
            }
        }
    }
    private void SpeedCheck()
    {
        // if falling increase speed of gravity
        if(charStats.FeetState == EFeetState.Falling)
        {
            charStats.gravitySpeed += charStats.gravityAcceleration;
            if(charStats.gravitySpeed > charStats.gravitySpeedMax)
            {
                charStats.gravitySpeed = charStats.gravitySpeedMax;
            }
        }
        // if onground reset speed o gravity
        if(charStats.FeetState == EFeetState.Onground)
        {
            charStats.ResetGravitySpeed();
        }
    }


}                   
