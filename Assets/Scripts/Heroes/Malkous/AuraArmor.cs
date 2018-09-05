using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraArmor : Ability
{



    [SerializeField]
    private float radius;
    [SerializeField]
    private float pushForce;
    [SerializeField]
    private float speed;


    private int layerMask;
    private int visibilityLayerMask;
    private float currentRadius;
    private bool abilityUse;

    public override void AbilityKeyPrssed()
    {
        Debug.Log(coolDownLock);
        if (!coolDownLock)
        {
            if (energyUsage <= charStats.Energy)
            {
                charStats.Energy -= energyUsage;

                if (layerMask == 0)
                {
                    layerMask = LayerMask.GetMask(charStats.enemyTeamName, charStats.teamName);
                    visibilityLayerMask = LayerMask.GetMask(charStats.enemyTeamName, charStats.teamName, "Blocks");
                }
                coolDownLock = true;
                charStats.HandState = EHandState.Casting;
                charStats.AbilityState = EAbility.Ability1Start;
                castTimeCoroutine = StartCoroutine(CastTime(castTime / charStats.SpeedRate));
                StartCoroutine(CoolDownTimer(coolDownTime));
            }
        }
    }

    protected override void AbilityCast()
    {
        charStats.HandState = EHandState.Idle;

        GetComponent<PlayerControl>().TakeAttack(0, buff.name);
        currentRadius = 0;
        abilityUse = true;
        //StartCoroutine(CoolDownTimer(coolDownTime));
    }

    private void FixedUpdate()
    { 
        if (abilityUse)
        {
            Collider2D[] hitObjects = Physics2D.OverlapCircleAll(transform.position, currentRadius + speed, layerMask, 0, 0);
            foreach (Collider2D obj in hitObjects)
            {
                if ((obj.gameObject.transform.position - transform.position).magnitude > currentRadius)
                {
                    if (Toolkit.IsVisible(transform.position, obj.transform.position, visibilityLayerMask,obj))
                    {
                        if (obj.gameObject.layer == gameObject.layer)
                        {
                            obj.gameObject.GetComponent<PlayerControl>().TakeAttack(0, buff.name);
                        }
                        else
                        {
                            float force = (radius - (obj.transform.position - transform.position).magnitude) / 8 + pushForce;
                            Vector2 direction = (obj.transform.position - transform.position).normalized;
                            obj.gameObject.GetComponent<CharacterPhysic>().AddReductiveForce(direction, force, 0.25f, 0);
                        }
                    }
                }
            }
            currentRadius += speed;
            if (currentRadius > radius)
            {
                abilityUse = false;
                charStats.AbilityState = EAbility.Ability2Finish;
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
