using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Ability : MonoBehaviour {
    public float coolDownTime;
    public float castTime;
    public float animationTime;
    protected bool coolDownLock;
    public Buff buff;
    protected CharacterAttributes charStats;
    public bool abilityUse { get; set; }

    void Awake()
    {
        abilityUse = false;
        charStats = GetComponent<CharacterAttributes>();
    }

    public abstract void AbilityKeyPrssed();
    public abstract void AbilityKeyHold();
    public abstract void AbilityKeyReleased();
    

}
