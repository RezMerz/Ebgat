using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public abstract class Ability : MonoBehaviour
{
    public float castTime;
    public float coolDownTime;
    public int energyUsage;


    protected bool coolDownLock;
    public Buff buff;
    public int abilityNumber;
    protected CharacterAttributes charStats;
    public bool abilityUseServerside { get; set; }
    public bool abilityUseClientside { get; set; }

    protected Coroutine castTimeCoroutine;

    protected void Awake()
    {
        abilityUseServerside = false;
        charStats = GetComponent<CharacterAttributes>();
    }

    public abstract void AbilityKeyPrssed();
    public abstract void AbilityKeyHold();
    public abstract void AbilityKeyReleased();

    public abstract void AbilityActivateClientSide();

    protected IEnumerator CoolDownTimer(float time)
    {
        yield return new WaitForSeconds(time);
        coolDownLock = false;
    }

    protected IEnumerator CastTime(float time)
    {
        yield return new WaitForSeconds(time);
        AbilityCast();
    }

    protected virtual void AbilityCast() { }
    public virtual void IntruptCast()
    {
        if (castTimeCoroutine != null)
        {
            StopCoroutine(castTimeCoroutine);
            charStats.HandState = EHandState.Idle;
            StartCoroutine(CoolDownTimer(coolDownTime));
        }
    }
}
