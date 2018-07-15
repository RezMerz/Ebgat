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
    public float armor;         //amount of reduced damage dealt to this character (%)

    public float hitPoints { get; set; }

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

    public  float moveSpeed {get;set;}
    public float moveAcceleration { get; set; }
    public float moveSpeedMax { get; set; }

    //jump attributes
    [SerializeField]
    private float jumpSpeedBase;
    [SerializeField]
    private float jumpAccelerationBase;
    [SerializeField]
    private float jumpSpeedMaxBase;

    public float jumpSpeed { get; set; }
    public float jumpAcceleration { get; set; }
    public float jumpSpeedMax { get; set; }

    //gravity attributes
    [SerializeField]
    private float gravitySpeedBase;
    [SerializeField]
    private float gravityAccelerationBase;
    [SerializeField]
    private float gravitySpeedMaxBase;

    public float gravitySpeed { get; set; }
    public float gravityAcceleration { get; set; }
    public float gravitySpeedMax { get; set; }


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
