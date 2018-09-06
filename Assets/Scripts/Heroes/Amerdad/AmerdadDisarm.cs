using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmerdadDisarm : Ability
{
    public float damage;
    public Vector2 size;
    public Vector2 offset;


    private Vector2 weaponSize;
    private float distance;
    private int layerMask;
    private int visibilityLayerMask;

    // Use this for initialization
    private void Start()
    {
        weaponSize = new Vector2(0.1f, size.y);
        distance = size.x;

    }
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
        StartCoroutine(CoolDownTimer(coolDownTime));
        offset = (charStats.Side + Vector2.up) * offset;
        RaycastHit2D[] targets = Physics2D.BoxCastAll(transform.position + (Vector3)offset, weaponSize, 0, charStats.Side, distance, layerMask, 0, 0);
        foreach (RaycastHit2D target in targets)
        {
            if (target.collider.tag.Equals("VirtualPlayer"))
            {
                if (Toolkit.IsVisible(transform.position,target.point,visibilityLayerMask,target.collider))
                {
                    target.collider.gameObject.GetComponent<PlayerControl>().TakeAttack(damage, buff.name);
                }
            }
        }
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
