using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousDash : CharacterDash {

    private MalkousAttack malkousAttack;


    private void Awake()
    {
        malkousAttack = GetComponent<MalkousAttack>();
    }
    // Update is called once per frame
    void Update () {
        if (started && charStats.BodyState == EBodyState.Dashing)
        {
            DashMove();
        }
	}

    private void DashMove()
    {
        float currentDistance = Time.deltaTime * speed * charStats.SpeedRate;
        if (currentDistance + distance <= range)
        {
            physic.AddForce(charStats.Side * speed * Time.deltaTime);
            physic.PhysicAction += MalkousDashHitFunction;
            distance += currentDistance;
        }
        else
        {
            physic.DashLayerReset();
            gameObject.layer = LayerMask.NameToLayer(charStats.teamName);
            charStats.BodyState = EBodyState.Standing;
            malkousAttack.StartIceShard();
            distance = 0;
        }
    }

    private void MalkousDashHitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {

    }
}
