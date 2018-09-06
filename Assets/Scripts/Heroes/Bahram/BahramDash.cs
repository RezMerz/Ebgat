using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BahramDash : CharacterDash
{
    private MeleeAttack meleeAttack;

    public float boarFormSpeed;
    public float boarFormRange;
    public float dashForce;
    public float stunAnimationTime;
    public Buff buff;

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
        if (charStats.AbilityState != EAbility.Ability2Start)
        {
            float currentDistance = Time.deltaTime * speed * charStats.SpeedRate;
            if (currentDistance + distance <= range)
            {
                physic.AddForce((charStats.Side.x * Vector2.right) * speed * Time.deltaTime);
                distance += currentDistance;
            }
            else
            {
                DashEnd();
            }
        }
        else
        {
            float currentDistance = Time.deltaTime * boarFormSpeed * charStats.SpeedRate;
            if (currentDistance + distance <= boarFormRange)
            {
                physic.AddForce((charStats.Side.x * Vector2.right) * speed * Time.deltaTime);
                physic.PhysicAction += BahramDashHitFunction;
                distance += currentDistance;
            }
            else
            {
                DashEnd();
            }
        }
    }

    protected override void DashStart()
    {
        gameObject.layer = LayerMask.NameToLayer("Dashing");
        if (charStats.AbilityState != EAbility.Ability1Start)
        {
            charStats.BodyState = EBodyState.Dashing;
        }
        else
        {
            charStats.FeetState = EFeetState.NoGravity;
            GetComponent<PlayerJump>().IntruptJump();
            physic.RemoveTaggedForces(0);
        }
    }

    public override void DashEnd()
    {
        if (charStats.AbilityState != EAbility.Ability2Start)
        {
            physic.DashLayerReset();
            gameObject.layer = LayerMask.NameToLayer(charStats.teamName);
        }
        else
        {
            charStats.FeetState = EFeetState.Onground;
        }
        charStats.BodyState = EBodyState.Standing;
        distance = 0;
        DashEffect();
    }

    protected override void DashEffect()
    {
        if (charStats.AbilityState != EAbility.Ability2Start)
        {
            charStats.AttackNumber = 2;
            physic.DashLayerSet();
        }
        else
        {
            //charStats.AttackNumber = 5;
        }
        meleeAttack.IntruptAttack();
        meleeAttack.StartComboCorutine();
    }

    private IEnumerator StunTime()
    {
        yield return new WaitForSeconds(stunAnimationTime);
        charStats.HandState = EHandState.Idle;
        charStats.AbilityState = EAbility.Ability3Finish;
    }

    private void BahramDashHitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        if (hHits.Count != 0 && direction.x * charStats.Side.x > 0)
        {
            if (hHits[0].collider.tag.Equals("VirtualPlayer"))
            {
                hHits[0].collider.gameObject.GetComponent<PlayerControl>().TakeAttack(0, buff.name);
                hHits[0].collider.gameObject.GetComponent<CharacterPhysic>().AddReductiveForce(charStats.Side, dashForce, 0.1f, 0);
                charStats.HandState = EHandState.Casting;
                charStats.AbilityState = EAbility.Ability3Start;
            }
            DashEnd();
        }
    }
}
