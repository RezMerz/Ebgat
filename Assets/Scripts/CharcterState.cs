using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharcterState : MonoBehaviour
{
    public enum BodyState { Conscious,Stunned };
    public enum ActionState {Idle,Moving,Attacking,Casting};
    public enum FeetState { Onground,Falling,Jumping};

    public BodyState bodyState;
    public ActionState actionState;
    public FeetState feetState; 
}
