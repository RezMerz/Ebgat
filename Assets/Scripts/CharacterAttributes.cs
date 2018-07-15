using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour {

    private EHeadState headState;
    private EBodyState bodyState;
    private EHandState handState;
    private EFeetState feetState;



    //health attributes
    [SerializeField]
    private float hitPointsMax;
    [SerializeField]
    private float armor;         //amount of reduced damage dealt to this character (%)

    private float hitPoints;

    //attack attributes
    [SerializeField]
    private float attackDamageBase;
    [SerializeField]
    private float attackAnimationTimeBase;
    [SerializeField]
    private float attackCooldownBase;

    private float attackDamage;
    private float attackAnimationTime;
    private float attackCooldown;

    //movement attributes
    [SerializeField]
    private float moveSpeedBase;
    [SerializeField]
    private float moveAccelerationBase;
    [SerializeField]
    private float moveSpeedMaxBase;

    private float moveSpeed;
    private float moveAcceleration;
    private float moveSpeedMax;

    //jump attributes
    [SerializeField]
    private float jumpSpeedBase;
    [SerializeField]
    private float jumpAccelerationBase;
    [SerializeField]
    private float jumpSpeedMaxBase;

    private float jumpSpeed;
    private float jumpAcceleration;
    private float jumpSpeedMax;

    //gravity attributes
    [SerializeField]
    private float gravitySpeedBase;
    [SerializeField]
    private float gravityAccelerationBase;
    [SerializeField]
    private float gravitySpeedMaxBase;

    private float gravitySpeed;
    private float gravityAcceleration;
    private float gravitySpeedMax;


    // Use this for initialization
    void Start ()
    {
        Initialize();
	}

    private void Initialize()
    {
        //Hp
        hitPoints = hitPointsMax;
        //Attack
        attackDamage = attackDamageBase;
        attackCooldown = attackCooldownBase;
        attackAnimationTime = attackAnimationTimeBase;
        //Move
        moveSpeed = moveSpeedBase;
        moveAcceleration = moveAccelerationBase;
        moveSpeedMax = moveSpeedMaxBase;
        //Jump
        jumpSpeed = jumpSpeedBase;
        jumpAcceleration = jumpAccelerationBase;
        jumpSpeedMax = jumpSpeedMaxBase;
        //Gravity
        gravitySpeed = gravitySpeedBase;
        gravityAcceleration = gravityAccelerationBase;
        gravitySpeedMax = gravitySpeedMaxBase;
    }
    //GetStates
    public EHeadState GetHeadState()
    {
        return headState;
    }
    public EBodyState GetBodyState()
    {
        return bodyState;
    }
    public EHandState GetHandState()
    {
        return handState;
    }
    public EFeetState GetFeetState()
    {
        return feetState;
    }
    //SetStates
    public void SetHeadState(EHeadState newSTate)
    {
        headState = newSTate;
    }
    public void SetBodyState(EBodyState newSTate)
    {
        bodyState = newSTate;
    }
    public void SetHandState(EHandState newSTate)
    {
        handState = newSTate;
    }
    public void SetFeetState(EFeetState newSTate)
    {
        feetState = newSTate;
    }
}



public enum EHeadState { Conscious, Stunned };
public enum EBodyState { Standing,Moveing};
public enum EHandState { Idle, Moving, Attacking, Casting };
public enum EFeetState { Onground, Falling, Jumping };
