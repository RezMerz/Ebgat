using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousAttack : Attack
{
    public float iceShardTime;

    [SerializeField]
    public List<MalkousBullet> virtualBullets;

    private int layerMask;
    private int playerID;

    private Coroutine iceShardCoroutine;
    private Coroutine animationTimeCoroutine;

    private new void Start()
    {
        charStats = GetComponent<CharacterAttributes>();
        playerControl = GetComponent<PlayerControl>();
        layerMask = LayerMask.GetMask("Blocks", charStats.enemyTeamName);
        ready = true;
    }

    public override void AttackPressed()
    {
        if (charStats.HeadState != EHeadState.Stunned && charStats.FeetState != EFeetState.OnWall)
        {
            Debug.Log(ready);
            if (ready)
            {
                if (charStats.Energy >= charStats.attackEnergyConsume)
                {
                    if (charStats.BodyState == EBodyState.Dashing)
                    {
                        GetComponent<CharacterDash>().DashEnd();
                    }
                    charStats.HandState = EHandState.Attacking;
                    animationTimeCoroutine = StartCoroutine(AttackAnimateTime(virtualBullets[charStats.AttackNumber].attackAnimationTime / charStats.SpeedRate));
                    ready = false;
                }
                else
                {
                    print(" Low Energy");
                }

            }
        }
    }

    protected override void ApplyAttack()
    {
        float damage = virtualBullets[charStats.AttackNumber].damage;
        float gravityAcc = charStats.GravityAcceleration;
        float range = virtualBullets[charStats.AttackNumber].range;
        int bulletID = ServerManager.instance.GetBulletID(playerControl.playerId);
        // Calculate Side
        Vector2 startPos = (charStats.Side + Vector2.up) * virtualBullets[charStats.AttackNumber].startingPos;
        Vector2 attackSide = charStats.AimSide;
        if (attackSide == Vector2.zero)
            attackSide = charStats.Side;
        GameObject virtualBullet = Instantiate(virtualBullets[charStats.AttackNumber].VirtualBullet, transform.position + (Vector3)startPos, Quaternion.identity);
        virtualBullet.layer = gameObject.layer;
        virtualBullet.GetComponent<VirtualBullet>().Shoot(damage, attackSide, layerMask, gravityAcc, playerControl, bulletID, range);
        // register bullet
        playerControl.worldState.BulletRegister(playerControl.playerId, bulletID, attackSide, gravityAcc, range, charStats.AttackNumber, startPos);

        if (charStats.AttackNumber != 0)
        {
            charStats.AttackNumber = 0;
        }

        StartCoroutine(CoolDown());
    }

    private IEnumerator IceShardTime()
    {
        charStats.AttackNumber = 1;
        yield return new WaitForSeconds(iceShardTime);
        charStats.AttackNumber = 0;
    }

    public void StartIceShard()
    {
        iceShardCoroutine = StartCoroutine(IceShardTime());
    }


    public override void IntruptAttack()
    {
        if (animationTimeCoroutine != null)
        {
            StopCoroutine(animationTimeCoroutine);
        }
        if (iceShardCoroutine != null)
        {
            StopCoroutine(iceShardCoroutine);
        }

        StartCoroutine(CoolDown());

    }
}

[System.Serializable]
public class MalkousBullet
{
    public GameObject VirtualBullet;
    public float range;
    public float attackAnimationTime;
    public float damage;
    public Vector2 startingPos;
}
