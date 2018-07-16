﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAttributes : MonoBehaviour {

    private EHeadState headState;

    public EHeadState HeadState
    {
        get { return headState; }
        set { headState = value; }
    }
    private EBodyState bodyState;

    public EBodyState BodyState
    {
        get { return bodyState; }
        set { bodyState = value; }
    }
    private EHandState handState;

    public EHandState HandState
    {
        get { return handState; }
        set { handState = value; }
    }
    private EFeetState feetState;

    public EFeetState FeetState
    {
        get { return feetState; }
        set { feetState = value; }
    }

   



    //health attributes
    [SerializeField]
    private float hitPointsMax;
    public float armor;         //amount of reduced damage dealt to this character (%)

    public float hitPoints { get; set; }

    //attack attributes
    public EAttackMode attackMode;
    [SerializeField]
    private float attackDamageBase;
    [SerializeField]
    private float attackAnimationTimeBase;
    [SerializeField]
    private float attackCooldownBase;

    [SerializeField]
    private float range;

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

    public float cayoteTime;
    public float gravitySpeed { get; set; }
    public float gravityAcceleration { get; set; }
    public float gravitySpeedMax { get; set; }

    // size attributes
    public Vector2 size { get; set; }
    // Use this for initialization
    void Start ()
    {
        
        Initialize();
	}

    private void Initialize()
    {
        //States 
        feetState = EFeetState.Onground;
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
        //Size
        size = GetComponent<BoxCollider2D>().size * transform.localScale;
    }

    public void ResetGravitySpeed()
    {
        gravitySpeed = gravitySpeedBase;
    }
}



public enum EHeadState { Conscious, Stunned };
public enum EBodyState { Standing,Moveing};
public enum EHandState { Idle, Moving, Attacking, Casting };
public enum EFeetState { Onground, Falling, Jumping };

public enum EAttackMode { Ranged,Melee};