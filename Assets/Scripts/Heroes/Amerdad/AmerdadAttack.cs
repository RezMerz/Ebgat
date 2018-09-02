using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmerdadAttack : Attack {

    public float maxHoldTime = 2;
    public float maxDamage = 100;
    private float currentHoldTime = 0;

    private Coroutine animationTimeCoroutine;

    private int layerMask;
    private int playerID;

    [SerializeField]
    public List<MalkousBullet> virtualBullets;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void AttackPressed()
    {
        if (charStats.HeadState != EHeadState.Stunned){
            if (ready)
            {
                if (charStats.Energy >= charStats.attackEnergyConsume)
                {
                    if (charStats.BodyState == EBodyState.Dashing)
                    {
                        GetComponent<CharacterDash>().DashEnd();
                    }
                    charStats.HandState = EHandState.Attacking;

                    ready = false;
                }
                else
                {
                    print(" Low Energy");
                }

            }
        }
    }

    IEnumerator AttackHoldCo(){
        while(currentHoldTime < maxHoldTime){
            currentHoldTime += Time.deltaTime;
            yield return null;
        }
    }

    public override void AttackReleased()
    {
        if (charStats.HandState == EHandState.Attacking)
        {
            animationTimeCoroutine = StartCoroutine(AttackAnimateTime(virtualBullets[charStats.AttackNumber].attackAnimationTime / charStats.SpeedRate));
        }
    }

    public override void IntruptAttack()
    {
        if (animationTimeCoroutine != null)
        {
            StopCoroutine(animationTimeCoroutine);
        }
        StartCoroutine(CoolDown());

    }

    protected override void ApplyAttack()
    {
        if (maxHoldTime < currentHoldTime)
            currentHoldTime = maxHoldTime;
        //float damage = virtualBullets[charStats.AttackNumber].damage;
        float damage = maxDamage * currentHoldTime / maxHoldTime;
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
}
