using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmerdadDash : CharacterDash
{

    private AmerdadAttack amerdadAttack;


    private void Awake()
    {

        amerdadAttack = GetComponent<AmerdadAttack>();
    }
    // Update is called once per frame
    void Update()
    {
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
            DashEnd();
        }
    }


    protected override void DashEffect()
    {
        amerdadAttack.StartIceShard();
        physic.DashLayerSet();
    }

    private void MalkousDashHitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {

    }
}
