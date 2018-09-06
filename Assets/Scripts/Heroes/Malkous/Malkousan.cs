using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Malkousan : Ability {
    public float interval;
    public int number;
    public int damage;

    private int currentNumber;
    private bool abilityUse;
    public float radius;
    private Coroutine intervalCo;
    private Coroutine ultiTime;
    private int layer;

    private Physic physic;

    void Start()
    {
        layer = LayerMask.GetMask(charStats.enemyTeamName);
        physic = GetComponent<Physic>();
    }
	// Use this for initialization
    public override void AbilityKeyPrssed()
    {
        if (charStats.HeadState == EHeadState.Conscious && (charStats.FeetState == EFeetState.Onground || charStats.FeetState == EFeetState.Root))
        {
            if (!coolDownLock)
            {
                if (energyUsage <= charStats.Energy)
                {
                    coolDownLock = true;
                    physic.Lock();
                    castTimeCoroutine = StartCoroutine(CastTime(castTime));
                    charStats.HandState = EHandState.Casting;
                    charStats.AbilityState = EAbility.Ability2Start;
                }
            }
        }
    }


    private IEnumerator IntervalCouroutine()
    {
        yield return new WaitForSeconds(interval);
        ShootShards();
        intervalCo = StartCoroutine(IntervalCouroutine());
    }

    private void ShootShards()
    {

    }

    public void FinishUlti()
    {
        charStats.AbilityState = EAbility.Ability2Finish;
        abilityUse = false;
        StopCoroutine(intervalCo);
    }

    public override void AbilityKeyHold()
    {

    }
    public override void AbilityKeyReleased()
    {

    }
    public override void AbilityActivateClientSide()
    {
        throw new System.NotImplementedException();
    }
}
