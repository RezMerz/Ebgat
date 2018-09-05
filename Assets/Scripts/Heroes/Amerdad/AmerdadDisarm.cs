using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmerdadDisarm : Ability {

    public Vector2 size;
    public Vector2 offset;

    private int layerMask;
    private int visibilityLayerMask;

    // Use this for initialization

    public override void AbilityKeyPrssed()
    {
        if (charStats.HeadState == EHeadState.Stunned)
        {
            return;
        }

        if (!coolDownLock)
        {
            if (energyUsage <= charStats.Energy)
            {
                if (charStats.FeetState != EFeetState.OnWall)
                {
                    if (charStats.HandState == EHandState.Attacking)
                    {
                        GetComponent<Attack>().IntruptAttack();
                    }
                    if (layerMask == 0)
                    {
                        layerMask = LayerMask.GetMask(charStats.enemyTeamName);
                        visibilityLayerMask = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
                    }
                    charStats.HandState = EHandState.Casting;
                    charStats.AbilityState = EAbility.Ability1Start;
                    castTimeCoroutine = StartCoroutine(CastTime(castTime / charStats.SpeedRate));
                }
            }
            else
            {
                Debug.Log("Not Enough Energy");
            }
        }
        else
        {
            Debug.Log("Cooldown");
        }
    }

    protected override void AbilityCast()
    {
        
    }

    public override void AbilityActivateClientSide()
    {
        
    }

    public override void AbilityKeyHold()
    {
        
    }

    public override void AbilityKeyReleased()
    {
        
    }
}
