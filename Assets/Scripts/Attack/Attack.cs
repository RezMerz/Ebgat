using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected CharacterAttributes charStats;
    protected PlayerControl playerControl;
    protected bool ready;
    // Use this for initialization
    protected void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        ready = true;
    }

    // Update is called once per frame
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
