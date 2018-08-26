using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Malkousan : Ability {
    private bool abilityUse;
    public float time;
    public float interval;
    public int damage;
    public Vector2 size;
    private Coroutine intervalCo;
    private Coroutine ultiTime;
    private int layer;

    void Start()
    {
        layer = LayerMask.GetMask(charStats.enemyTeamName, "Dummy");
        print(layer);
    }
	// Use this for initialization
    public override void AbilityKeyPrssed()
    {
        if (!coolDownLock)
        {
             coolDownLock = true;
             abilityUse = true;
             charStats.AbilityState = EAbility.Ability2Start;
             StartCoroutine(CoolDownTimer(coolDownTime));
             ultiTime = StartCoroutine(UltiTime());
             intervalCo = StartCoroutine(IntervalCouroutine());
        }
    }


    private IEnumerator IntervalCouroutine()
    {
        yield return new WaitForSeconds(interval);
        DamageEnemies();
        intervalCo = StartCoroutine(IntervalCouroutine());
    }

    private void DamageEnemies()
    {
        Collider2D[] hitObjects = Physics2D.OverlapBoxAll(transform.position, size, 0, layer);
        foreach (Collider2D hit in hitObjects)
        {
            hit.GetComponent<PlayerControl>().TakeAttack(damage, buff.name);
        }
        
    }
    private IEnumerator UltiTime()
    {
        yield return new WaitForSeconds(time);
        FinishUlti();
    }

    public void FinishUlti()
    {
        charStats.AbilityState = EAbility.Ability2Finish;
        abilityUse = false;
        StopCoroutine(intervalCo);
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
