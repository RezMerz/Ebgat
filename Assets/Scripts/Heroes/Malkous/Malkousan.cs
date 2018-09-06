using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Malkousan : Ability {
    public GameObject virtualBullet;

    public float interval;
    public int number;
    public int damage;
    public float maxHeight;
    public float minHeight;
    private int currentNumber;
    public float radius;
    private Coroutine intervalCo;
    private Coroutine ultiTime;
    private int layer;

    private Physic physic;
    private PlayerControl playerControl;

    void Start()
    {
        physic = GetComponent<Physic>();
        playerControl = GetComponent<PlayerControl>();
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
                    if(layer == 0)
                    {
                        layer = LayerMask.GetMask(charStats.enemyTeamName);
                        Debug.Log(layer);
                    }
                    coolDownLock = true;
                    physic.Lock();
                    castTimeCoroutine = StartCoroutine(CastTime(castTime));
                    currentNumber = 0;
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
    }

    private void ShootShards()
    {
        Debug.Log(currentNumber);
        float gravityAcc = charStats.GravityAcceleration;
        int bulletID = ServerManager.instance.GetBulletID(playerControl.playerId);
        // Calculate Side
        Vector2 startPos = new Vector2(Random.Range(-radius, radius), maxHeight - transform.position.y);
        Debug.Log(startPos);
        float range = Random.Range(minHeight, maxHeight);
        GameObject virtualBullet = Instantiate(this.virtualBullet, transform.position + (Vector3)startPos, Quaternion.identity);
       
        virtualBullet.layer = gameObject.layer;
        virtualBullet.GetComponent<VirtualBullet>().Shoot(damage,Vector2.down, layer, gravityAcc, playerControl, bulletID, 0, range);
        // register bullet
        playerControl.worldState.BulletRegister(playerControl.playerId, bulletID, Vector2.down, gravityAcc,0,2, startPos, range);
        currentNumber++;
        if(currentNumber < number)
        {
            Debug.Log(number);
            intervalCo = StartCoroutine(IntervalCouroutine());
        }
    }

    protected override void AbilityCast()
    {
        intervalCo = StartCoroutine(IntervalCouroutine());
        StartCoroutine(CoolDownTimer(coolDownTime));
        charStats.HandState = EHandState.Idle;
        physic.Unlock();
    }
    public override void IntruptCast()
    {
        base.IntruptCast();
        physic.Unlock();
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
