using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected float damage;
    protected float cooldownTimer;
    protected CharacterAttributes charStats;
    protected HeroGraphics heroGraphics;
    protected PlayerControl playerControl;
    // Use this for initialization
    protected void Start()
    {
        heroGraphics = GetComponent<HeroGraphics>();
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        damage = charStats.AttackDamage;
        cooldownTimer = 0;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;
    }

    protected IEnumerator AttackAnimateTime()
    {
        yield return new WaitForSeconds(charStats.AttackAnimationTime);
        charStats.HandState = EHandState.Idle;
    }
    public virtual void AttackPressed() { }

    public virtual void AttackHold() { }

    public virtual void AttackReleased() { }

    public virtual void AttackClientside(Vector2 direction, int attackID)
    {

    }
}
