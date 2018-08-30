using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousDash : CharacterDash {


	
	// Update is called once per frame
	void Update () {
        if (started && charStats.BodyState == EBodyState.Dashing)
        {
            DashMove();
        }
	}

    private void DashMove()
    {
        float currentDistance = Toolkit.FloatCut(Time.deltaTime * speed);
        if (currentDistance + distance <= range)
        {
            physic.AddForce(charStats.Side * speed * Time.deltaTime);
            physic.PhysicAction += MalkousDashHitFunction;
            distance += Toolkit.FloatCut(speed * Time.deltaTime);
        }
        else
        {
            physic.DashLayerReset();
            charStats.BodyState = EBodyState.Standing;
            distance = 0;
        }
    }

    private void MalkousDashHitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {

    }
}
