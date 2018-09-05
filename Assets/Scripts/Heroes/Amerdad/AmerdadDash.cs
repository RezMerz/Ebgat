using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmerdadDash : CharacterDash {

    protected bool secondCoolDownLock;

	void Update () 
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
            distance += currentDistance;
        }
        else
        {
            DashEnd();
        }
    }

    public override void DashPressed()
    {
        if(charStats.BodyState == EBodyState.Dashing)
            return;
        if (!coolDownLock)
        {
            if (charStats.Energy >= charStats.dashEnergyConsume)
            {
                if (charStats.HandState == EHandState.Attacking)
                {
                    attack.IntruptAttack();
                }
                //StartFunction();
                coolDownLock = true;
                StartCoroutine(CoolDownReset());
                charStats.BodyState = EBodyState.Dashing;
                physic.DashLayerSet();
                gameObject.layer = LayerMask.NameToLayer("Dashing");
            }
            else
            {
                print("Low Energy");
            }
        }
        else if(!secondCoolDownLock){
            if (charStats.Energy >= charStats.dashEnergyConsume)
            {
                if (charStats.HandState == EHandState.Attacking)
                {
                    attack.IntruptAttack();
                    charStats.BodyState = EBodyState.Dashing;
                    StartCoroutine(SecondCoolDownReset());
                    physic.DashLayerSet();
                    gameObject.layer = LayerMask.NameToLayer("Dashing");
                }
                secondCoolDownLock = true;
            }
            else
            {
                print("Low Energy");
            }
        }
    }

    private IEnumerator SecondCoolDownReset()
    {
        yield return new WaitForSeconds(coolDownTime);
        secondCoolDownLock = false;
    }

    public override void DashEnd()
    {
        base.DashEnd();
    }
}
