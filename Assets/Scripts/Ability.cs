using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public abstract class Ability : NetworkBehaviour
{
    public float coolDownTime;
    public float castTime;
    public float animationTime;
    protected bool coolDownLock;
    public Buff buff;

    protected CharacterAttributes charStats;
    public bool abilityUseServerside { get; set; }
    public bool abilityUseClientside { get; set; }

    void Awake()
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

}
