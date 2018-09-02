using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected float cooldownTimer;
    protected CharacterAttributes charStats;
    protected HeroGraphics heroGraphics;
    protected PlayerControl playerControl;
    protected bool ready;
    // Use this for initialization
    protected void Start()
    {
        heroGraphics = GetComponent<HeroGraphics>();
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        ready = true;
        cooldownTimer = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
    }

    protected IEnumerator AttackAnimateTime(float attackAnimationTime)
    {
        yield return new WaitForSeconds(attackAnimationTime);
        ApplyAttack();
        charStats.HandState = EHandState.Idle;
    }
    protected IEnumerator CoolDown()
    {
        yield return new WaitForSeconds(charStats.AttackCooldown);
        ready = true;
    }

    protected virtual void ApplyAttack() { }

    public virtual void AttackPressed() { }

    public virtual void AttackHold() { }

    public virtual void AttackReleased() { }

    public virtual void IntruptAttack() { }

    public virtual void AttackClientside(Vector2 direction, int attackID)
    {

    }
}
