using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BahramDash : CharacterDash
{
    private MeleeAttack meleeAttack;


    private new void Start()
    {
        base.Start();
        meleeAttack = GetComponent<MeleeAttack>();
    }

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
        float currentDistance = Time.deltaTime * speed * charStats.SpeedRate;
        if (currentDistance + distance <= range)
        {
            physic.AddForce((charStats.Side.x * Vector2.right) * speed * Time.deltaTime);
            physic.PhysicAction += BahramDashHitFunction;
            distance += currentDistance;
        }
        else
        {
            physic.DashLayerReset();
            charStats.AttackNumber = meleeAttack.maxAttackNumber - 1;
            meleeAttack.IntruptAttack();
            meleeAttack.StartComboCorutine();
            gameObject.layer = LayerMask.NameToLayer(charStats.teamName);
            charStats.BodyState = EBodyState.Standing;
            distance = 0;
        }
    }

    private void BahramDashHitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {

    }
}
