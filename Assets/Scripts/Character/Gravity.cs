using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {

    private PlayerControl playerControl;
    private CharacterPhysic physic;
    private CharacterAttributes charStats;

    private float timer;

   
    // Use this for initialization
    void Start()
    {
        physic = GetComponent<CharacterPhysic>();
        playerControl = GetComponent<PlayerControl>();
        charStats = GetComponent<CharacterAttributes>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControl.IsServer())
        {
            GravityServerside();
        }
    }
    private void GravityServerside()
    {
        SpeedCheck();
        Vector2 force = Vector2.down * (charStats.GravitySpeed * Time.deltaTime);
        //Debug.Log(force);
        physic.AddForce(force);
        physic.PhysicAction += HitFunction;
    }
    private void SpeedCheck()
    {
        if(charStats.FeetState == EFeetState.Onground)
        {
            charStats.ResetGravitySpeed();
        }
        else
        {
            charStats.GravitySpeed += charStats.GravityAcceleration * Time.deltaTime;
        }
    }

    private void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        if (vHits.Count > 0 && direction.y < 0)
        {
            Debug.Log(timer);
            timer = 0;
            charStats.FeetState = EFeetState.Onground;
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
