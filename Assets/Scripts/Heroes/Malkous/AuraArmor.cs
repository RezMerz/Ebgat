using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraArmor : Ability
{

    [SerializeField]
    private float radius;

    private int layerMask;
    private float currentRadius;
    private bool abilityUse;

    public override void AbilityKeyPrssed()
    {
        if (!coolDownLock)
        {
            if (energyUsage <= charStats.Energy)
            {
                if (layerMask == 0)
                {
                    layerMask = LayerMask.GetMask(charStats.enemyTeamName,charStats.teamName);
                }
                coolDownLock = true;
                charStats.HandState = EHandState.Casting;
                charStats.AbilityState = EAbility.Ability1Start;
                castTimeCoroutine = StartCoroutine(CastTime(castTime/charStats.SpeedRate));
                StartCoroutine(CoolDownTimer(coolDownTime));
            }
        }
    }

    protected override void AbilityCast()
    {
        abilityUse = true;
    }

    void Update()
    {
        if (abilityUse)
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, radius, LayerMask.GetMask(charStats.teamName));
            foreach (Collider2D obj in hitObjects)
            {
                //obj.GetComponent<PlayerControl>().TakeAttack(0, buff.name);
            }
        }
    }

    public override void AbilityKeyHold()
    {

    }
    public override void AbilityKeyReleased()
    {

    }
    public override void AbilityActivateClientSide()
    {
    }
}
