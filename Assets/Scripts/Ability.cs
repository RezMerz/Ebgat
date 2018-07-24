using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour {
    public float coolDown;
    public float castTime;
    public float animationTime;
    public Buff buff;

    public abstract void AbilityKeyPrssed();
    public abstract void AbilityKeyHold();
    public abstract void AbilityKeyReleased();
    

}
