using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmerdadAttack : Attack
{


    public float minDamage;
    public float maxDamage;

    public float maxHoldTime = 2;
    private float currentHoldTime = 0;

    private Coroutine animationTimeCoroutine;
    private Coroutine attackHoldCoroutine;
    private Coroutine thornedArrow;

    public float thronedArrowTime;

    private int layerMask;
    private int playerID;

    [SerializeField]
    public List<MalkousBullet> virtualBullets;


    
    // Use this for initialization

    public override void AttackPressed()
    {
        if (charStats.HeadState != EHeadState.Stunned && !charStats.Disarm)
        {
            if (ready)
            {
                if (charStats.Energy >= charStats.attackEnergyConsume)
                {
                    if (charStats.BodyState == EBodyState.Dashing)
                    {

                        GetComponent<CharacterDash>().DashEnd();
                    }

                    if(layerMask == 0)
                    {
                        layerMask = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
                    }

                    charStats.HandState = EHandState.Channeling;
                    attackHoldCoroutine = StartCoroutine(AttackHoldCo());
                    ready = false;
                }
                else
                {
                    print(" Low Energy");
                }
            }
        }
    }

    IEnumerator AttackHoldCo()
    {
        while (currentHoldTime < maxHoldTime)
        {
            currentHoldTime += Time.deltaTime;
            yield return null;
        }
    }

    public override void AttackReleased()
    {
        if (charStats.HandState == EHandState.Channeling)
        {
            if (attackHoldCoroutine != null)
            {
                StopCoroutine(attackHoldCoroutine);
            }
            charStats.HandState = EHandState.Attacking;
            animationTimeCoroutine = StartCoroutine(AttackAnimateTime(virtualBullets[charStats.AttackNumber].attackAnimationTime / charStats.SpeedRate));
        }
    }

    public override void IntruptAttack()
    {
        if (animationTimeCoroutine != null)
        {
            StopCoroutine(animationTimeCoroutine);
        }
        if (attackHoldCoroutine != null)
        {
            StopCoroutine(attackHoldCoroutine);
        }
        StartCoroutine(CoolDown());
    }

    protected override void ApplyAttack()
    {
        if (maxHoldTime < currentHoldTime)
        {
            currentHoldTime = maxHoldTime;
        }
        float damage = minDamage + (currentHoldTime / maxHoldTime) * (maxDamage - minDamage);
        float gravityAcc = charStats.GravityAcceleration;
        float range = virtualBullets[charStats.AttackNumber].range;
        int bulletID = ServerManager.instance.GetBulletID(playerControl.playerId);
        // Calculate Side
        Vector2 startPos = (charStats.Side + Vector2.up) * virtualBullets[charStats.AttackNumber].startingPos;
        Vector2 attackSide = charStats.AimSide;
        if (attackSide == Vector2.zero)
        {
            if (charStats.FeetState == EFeetState.OnWall)
            {
                attackSide = charStats.Side * -1;
            }
            else
            {
                attackSide = charStats.Side;
            }
        }
        GameObject virtualBullet = Instantiate(virtualBullets[charStats.AttackNumber].VirtualBullet, transform.position + (Vector3)startPos, Quaternion.identity);
        virtualBullet.layer = gameObject.layer;
        virtualBullet.GetComponent<VirtualBullet>().Shoot(damage, attackSide, layerMask, gravityAcc, playerControl, bulletID, range,0);
        // register bullet
        playerControl.worldState.BulletRegister(playerControl.playerId, bulletID, attackSide, gravityAcc, range, charStats.AttackNumber, startPos,0);
        StartCoroutine(CoolDown());
        currentHoldTime = 0;
        if (charStats.AttackNumber != 0)
        {
            charStats.AttackNumber = 0;
        }
    }

    private IEnumerator ThornedArrowTime()
    {
        charStats.AttackNumber = 1;
        yield return new WaitForSeconds(thronedArrowTime);
        charStats.AttackNumber = 0;
    }

    public void StartIceShard()
    {
        thornedArrow = StartCoroutine(ThornedArrowTime());
    }
}
