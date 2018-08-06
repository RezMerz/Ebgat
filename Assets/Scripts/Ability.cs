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

    public abstract void AbilityKeyPrssedServerSide();
    public abstract void AbilityKeyHold();
    public abstract void AbilityKeyReleased();
}
