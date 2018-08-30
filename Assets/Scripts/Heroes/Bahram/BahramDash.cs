using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BahramDash : CharacterDash
{

    // Use this for initialization
    void Update()
    {
        if (started)
        {
           // Debug.Log(charStats.BodyState);
            if (charStats.BodyState == EBodyState.Dashing)
            {

                DashMove();
            }
        }
    }

    // Update is called once per frame
    private void DashMove()
    {
        float currentDistance = Toolkit.FloatCut(Time.deltaTime * speed);
        if (currentDistance + distance <= range)
        {
            physic.AddForce(charStats.Side * speed * Time.deltaTime);
            physic.PhysicAction += BahramDashHitFunction;
            distance += Toolkit.FloatCut(speed * Time.deltaTime);
        }
        else
        {
            physic.DashLayerReset();
            charStats.BodyState = EBodyState.Standing;
            distance = 0;
        }
    }

    private void BahramDashHitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {

    }
}
