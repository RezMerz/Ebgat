using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineCharacterGravity : MonoBehaviour {

    private CharacterAttributes charStats;
    private List<RaycastHit2D> hitObjects;
    private Transform transformHero;
    private float timer;
    private int mask;


    // Use this for initialization
    private void Start()
    {
        mask = LayerMask.GetMask("Blocks", "Bridge");
        transformHero = transform;
        hitObjects = new List<RaycastHit2D>();
        charStats = GetComponent<CharacterAttributes>();
    }
    // Update is called once per frame
    private void Update()
    {
        SpeedCheck();
        HeroGravity();
    }
    private void HeroGravity()
    {
        // not int jumping state
        if (charStats.FeetState != EFeetState.Jumping)
        {
            bool hit = Toolkit.CheckMove(transformHero.position, charStats.size, Vector2.down, Time.deltaTime * charStats.GravitySpeed, mask, out hitObjects);

            if (charStats.FeetState == EFeetState.Onground)
            {
                // Go to Falling
                if (!hit)
                {
                    if (timer >= charStats.CayoteTime)
                    {
                        charStats.FeetState = EFeetState.Falling;
                        transformHero.position += Vector3.down * (Time.deltaTime * charStats.GravitySpeed);
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
                if (hit)
                {
                    var hitObject = hitObjects[0];

                    if(hitObject.collider.gameObject.layer != LayerMask.NameToLayer("Bridge"))
                    {
                        transformHero.position += Vector3.down * (hitObject.distance);
                        charStats.FeetState = EFeetState.Onground;
                    }
                    else
                    {
                        if(hitObject.point.y == hitObject.collider.transform.position.y + hitObject.collider.transform.localScale.y / 2)
                        {
                            transformHero.position += Vector3.down * (hitObject.distance);
                            charStats.FeetState = EFeetState.Onground;
                        }
                        else
                        {
                            transformHero.position += Vector3.down * (Time.deltaTime * charStats.GravitySpeed);
                        }
                    }
                }
                // Still Faliing
                else
                {
                    transformHero.position += Vector3.down * (Time.deltaTime * charStats.GravitySpeed);
                }
            }
        }
    }
    private void SpeedCheck()
    {
        // if falling increase speed of gravity
        if (charStats.FeetState == EFeetState.Falling)
        {
            charStats.GravitySpeed += charStats.GravityAcceleration * Time.deltaTime;
            if (charStats.GravitySpeed > charStats.GravitySpeedMax)
            {
                charStats.GravitySpeed = charStats.GravitySpeedMax;
            }
        }
        // if onground reset speed o gravity
        if (charStats.FeetState == EFeetState.Onground)
        {
            charStats.ResetGravitySpeed();
        }
    }

    public void IncludeBridge()
    {
        mask = LayerMask.GetMask("Blocks", "Bridge");
    }
    public void ExcludeBridge()
    {
        mask = LayerMask.GetMask("Blocks");
        timer = charStats.CayoteTime;
    }
}
