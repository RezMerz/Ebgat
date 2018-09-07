using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmerdadRoot : Ability
{
    private CharacterPhysic physic;

    // Use this for initialization
    void Start()
    {
        physic = GetComponent<CharacterPhysic>();
    }

    public override void AbilityActivateClientSide()
    {
        throw new System.NotImplementedException();
    }

    public override void AbilityKeyHold()
    {

    }

    public override void AbilityKeyPrssed()
    {
        if (charStats.HeadState == EHeadState.Conscious && (charStats.FeetState == EFeetState.Onground))
        {
            if (!coolDownLock)
            {
                if (charStats.Rage == charStats.maxRage)
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

    protected override void AbilityCast()
    {
        charStats.EmptyRage();
        GameObject[] playerobj = GameObject.FindGameObjectsWithTag("VirtualPlayer");
        for (int i = 0; i < playerobj.Length; i++)
        {
            PlayerControl playerControl = playerobj[i].GetComponent<PlayerControl>();
            if (!playerControl.charStats.teamName.Equals(charStats.teamName))
            {
                playerControl.TakeAttack(playerControl,0, buff.name);
            }
        }
        physic.Unlock();
        charStats.HandState = EHandState.Idle;
        charStats.AbilityState = EAbility.Ability2Finish;
        StartCoroutine(CoolDownTimer(coolDownTime));
    }

    public override void AbilityKeyReleased()
    {

    }
}
