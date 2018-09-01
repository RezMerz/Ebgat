﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    public GameObject weaponObject;
    public float attackForce;
    private Vector2 weaponSize;
    private float distance;
    private int layerMask;
    private string buffName;
    private MeleeWaepon weapon;
    private GameObject sword;

    //Combo
    public float[] attackAnimationTimes;
    public int maxAttackNumber { get; private set; }
    [SerializeField]
    private float comboTime;
    private Coroutine comboTimeCoroutine;
    private Coroutine parryTimeCoroutine;
    private Coroutine animationTimeCoroutine;
    private new void Start()
    {
        base.Start();
        weapon = weaponObject.GetComponent<MeleeWaepon>();
        weaponSize = new Vector2(transform.localScale.x * GetComponent<BoxCollider2D>().size.x * 0.5f, weapon.size.y);
        sword = Instantiate(weaponObject, transform.position + Vector3.right * ((transform.localScale.x * GetComponent<BoxCollider2D>().size.x + weapon.size.x) * 0.5f), Quaternion.identity);
        sword.layer = gameObject.layer;
        sword.transform.parent = gameObject.transform;
        sword.GetComponent<BoxCollider2D>().enabled = false;
        distance = weapon.size.x;
        layerMask = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
        if (weapon.buff != null)
        {
            buffName = weapon.buff.name;
        }
        else
        {
            buffName = "";
        }

        maxAttackNumber = attackAnimationTimes.Length;
    }

    public override void AttackPressed()
    {
        if (charStats.HeadState != EHeadState.Stunned)
        {
            if (cooldownTimer <= 0)
            {
                if (charStats.Energy >= charStats.attackEnergyConsume)
                {
                    cooldownTimer = charStats.AttackCooldown;
                    charStats.HandState = EHandState.Attacking;
                    if (comboTimeCoroutine != null)
                    {
                        StopCoroutine(comboTimeCoroutine);
                    }
                    parryTimeCoroutine = StartCoroutine(ParryTime());
                    animationTimeCoroutine = StartCoroutine(AttackAnimateTime(attackAnimationTimes[charStats.AttackNumber] / charStats.SpeedRate));
                }
                else
                {
                    print("Low Energy");
                }
            }
        }
    }
    private IEnumerator ParryTime()
    {
        sword.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(charStats.AttackAnimationTime * 2f / charStats.SpeedRate);
        sword.GetComponent<BoxCollider2D>().enabled = false;
    }

    protected override void ApplyAttack()
    {
        // StartCoroutine(ParryTime());
        RaycastHit2D[] targets = Physics2D.BoxCastAll(transform.position, weaponSize, 0, charStats.Side, distance, layerMask, 0, 0);
        bool parry = false;
        foreach (RaycastHit2D target in targets)
        {
            if (target.collider.tag.Equals("Sword"))
            {
                Debug.Log("parry");
                parry = true;
                target.collider.gameObject.GetComponentInParent<CharacterPhysic>().AddPersistentForce(charStats.Side * (attackForce * 60), 2, 2);
                playerControl.physic.AddPersistentForce(charStats.Side * (-attackForce * 60), 2, 2);
            }
        }
        if (!parry)
        {
            foreach (RaycastHit2D target in targets)
            {
                if (target.collider.tag.Equals("Player"))
                {
                    target.collider.gameObject.GetComponent<PlayerControl>().TakeAttack(charStats.AttackDamage, buffName);
                    target.collider.gameObject.GetComponent<CharacterPhysic>().AddForce(charStats.Side * attackForce);
                }
                else
                {
                    playerControl.physic.AddForce(charStats.Side * -attackForce);
                }
            }
        }
        charStats.AttackNumber = (charStats.AttackNumber + 1) % maxAttackNumber;
        if (charStats.AttackNumber != 0)
        {
            comboTimeCoroutine = StartCoroutine(ComboTime());
        }
        // print(charStats.AttackDamage);
    }

    public IEnumerator ComboTime()
    {
        yield return new WaitForSeconds(comboTime);
        charStats.AttackNumber = 0;
    }

    public override void IntruptAttack()
    {
        if(animationTimeCoroutine != null)
        {
            StopCoroutine(animationTimeCoroutine);
        }
        if(parryTimeCoroutine != null)
        {
            StopCoroutine(parryTimeCoroutine);
            sword.GetComponent<BoxCollider2D>().enabled = false;
        }
        if(comboTimeCoroutine != null)
        {
            StopCoroutine(comboTimeCoroutine);
        }
        charStats.HandState = EHandState.Idle;
    }
    
    public void StartComboCorutine()
    {
        StartCoroutine(ComboTime());
    }
}
