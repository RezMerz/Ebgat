using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrushOfConqueror : Ability
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float stunDuration;
    [SerializeField]
    private float Speed;
    [SerializeField]
    private float landingWidth;
    [SerializeField]
    private float pushDownWidth;
    [SerializeField]
    private float pushForce;

    private CharacterPhysic physic;
    private Vector2 pushDownSize;
    private Vector2 landingSize;
    private Vector2 characterSize;
    private int layerMask;
    private int visibilityLayerMask;
    private bool active;


    private void Start()
    {
        physic = GetComponent<CharacterPhysic>();
        characterSize = transform.localScale * GetComponent<BoxCollider2D>().size;
        pushDownSize = new Vector2(pushDownWidth, characterSize.y / 2);
        landingSize = new Vector2(landingWidth, 0.1f);
    }

    public override void AbilityKeyPrssed()
    {
        if(charStats.HeadState == EHeadState.Stunned)
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
                        GetComponent<MeleeAttack>().IntruptAttack();
                    }
                    if (layerMask == 0)
                    {
                        layerMask = LayerMask.GetMask(charStats.enemyTeamName);
                        visibilityLayerMask = LayerMask.GetMask(charStats.enemyTeamName,"Blocks");
                    }

                    physic.Lock();

                    charStats.HandState = EHandState.Casting;
                    charStats.AbilityState = EAbility.Ability1Start;
                    coolDownLock = true;
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
        active = true;
        physic.DashLayerSet();
        // physic.ExcludeBridge();

    }

    public override void IntruptCast()
    {
        if (castTimeCoroutine != null)
        {
            StopCoroutine(castTimeCoroutine);
            physic.Unlock();
            StartCoroutine(CoolDownTimer(coolDownTime));
        }
    }
    private void FixedUpdate()
    {
        if (active)
        {
            physic.AddForce(Vector2.down * (Time.deltaTime * Speed));
            physic.PhysicAction += HitFunction;
        }
    }

    private void PushDown(float distance)
    {
        RaycastHit2D[] enemies = Physics2D.BoxCastAll(transform.position + (Vector3.down * characterSize.y / 4), pushDownSize, 0, Vector2.down, distance, layerMask, 0, 0);
        foreach (RaycastHit2D hit in enemies)
        {
            if (hit.collider.tag.Equals("Player"))
            {
                GameObject enemy = hit.collider.gameObject;
                enemy.GetComponent<CharacterPhysic>().AddForce(Vector2.down * distance);
            }
        }
    }

    private void LandCrush()
    {
        Vector2 origin = transform.position + (Vector3.up * characterSize.y * 3 / 2);


        Debug.Log(landingSize);
        Debug.DrawLine(origin, origin + Vector2.down * characterSize.y * 2, Color.red, 5);
        RaycastHit2D[] enemies = Physics2D.BoxCastAll(origin, landingSize, 0, Vector2.down, characterSize.y * 2, layerMask, 0, 0);
        Debug.Log(enemies.Length);
        foreach (RaycastHit2D hit in enemies)
        {
            if (hit.collider.tag.Equals("VirtualPlayer"))
            {
                GameObject enemy = hit.collider.gameObject;
                if (enemy.transform.position.x > transform.position.x ) 
                {
                    if (Toolkit.IsVisible(transform.position, hit.point, visibilityLayerMask, hit.collider))
                    {
                        float force = (landingSize.x/2 - Mathf.Abs((hit.point - (Vector2)transform.position).x)) / 9 + pushForce;
                        enemy.GetComponent<CharacterPhysic>().AddReductiveForce(Vector2.right,force,0.25f,0);
                        enemy.GetComponent<PlayerControl>().TakeAttack(damage, buff.name);
                    }
                }
                else if(enemy.transform.position.x <= transform.position.x )
                {
                    if (Toolkit.IsVisible(transform.position, hit.point, visibilityLayerMask,hit.collider))
                    {
                        float force = (landingSize.x / 2 - Mathf.Abs((hit.point - (Vector2)transform.position).x)) / 9 + pushForce;
                        enemy.GetComponent<CharacterPhysic>().AddReductiveForce(Vector2.left, force, 0.25f, 0);
                        enemy.GetComponent<PlayerControl>().TakeAttack(damage, buff.name);
                    }
                }
            }
        }
        StartCoroutine(CoolDownTimer(coolDownTime));
        charStats.HandState = EHandState.Idle;
        charStats.AbilityState = EAbility.Ability1Finish;

        physic.Unlock();
        physic.DashLayerReset();
        //physic.IncludeBridge();
        active = false;
    }
    private void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        if (direction.y < 0)
        {
            PushDown(-direction.y);
            if (vHits.Count > 0)
            {
                LandCrush();
            }
        }
    }
    public override void AbilityKeyHold()
    {
        throw new NotImplementedException();
    }
    public override void AbilityKeyReleased()
    {
        throw new NotImplementedException();
    }
    public override void AbilityActivateClientSide()
    {
        throw new NotImplementedException();
    }
}
