using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour {

    private EHeadState headState;
    private EBodyState bodyState;
    private EHandState handState;
    private EFeetState feetState;



    //health attributes
    public float maxHitPoints;
    public float armor;                        //amount of reduced damage dealt to this character (%)

    private float hitPoints;

    //attack attributes
    public float attackDamageBase;
    public float attackAnimationTimeBase;
    public float attackCooldownBase;

    private float attackDamage;
    private float attackAnimationTime;
    private float attackCooldown;

    //movement attributes
    public float moveSpeedBase;
    public float moveAccelerationBase;
    public float moveSpeedMaxBase;

    private float moveSpeed;
    private float moveAcceleration;
    private float moveSpeedMax;

    //jump attributes
    public float jumpSpeedBase;
    public float jumpAccelerationBase;
    public float jumpSpeedMaxBase;

    private float jumpSpeed;
    private float jumpAcceleration;
    private float jumpSpeedMax;

    //gravity attributes
    public float gravitySpeedBase;
    public float gravityAccelerationBase;
    public float gravitySpeedMaxBase;

    private float gravitySpeed;
    private float gravityAcceleration;
    private float gravitySpeedMax;


    // Use this for initialization
    void Start ()
    {
		
	}
}
public enum EHeadState { Conscious, Stunned };
public enum EBodyState { Standing,Moveing};
public enum EHandState { Idle, Moving, Attacking, Casting };
public enum EFeetState { Onground, Falling, Jumping };
