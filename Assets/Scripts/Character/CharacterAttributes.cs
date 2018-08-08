using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Globalization;
public class CharacterAttributes : MonoBehaviour {

    private PlayerControl playerControl;
    int a;

    private EHeadState headState; //a
    
    public EHeadState HeadState
    {
        get { return headState; }
        set { headState = value; RegisterHeadState(); }
    }

    private EBodyState bodyState; //b

    StackTrace stackTrace = new StackTrace();
    public EBodyState BodyState
    {
        get { return bodyState; }
        set { bodyState = value; RegisterBodyState(); }
    }
    private EHandState handState; //c

    public EHandState HandState
    {
        get { return handState; }
        set { handState = value; RegisterHandState(); }
    }
    private EFeetState feetState; //d

    public EFeetState FeetState
    {
        get { return feetState; }
        set { feetState = value; RegisterFeetState(); }
    }
    public string teamName, enemyTeamName;
   
    public Vector2 side {
        get { return side; }
        set { side = value; playerControl.worldState.RegisterCharStat('e', Toolkit.VectorSerialize(value)); } 
    } //e

    //health attributes
    [SerializeField]
    private float hitPointsBase;
    public float armor{
        get { return armor; }
        set { armor = value; playerControl.worldState.RegisterCharStat('f', value + ""); }
    } //f         //amount of reduced damage dealt to this character (%)

    public float hitPoints {
        get { return hitPoints; }
        set { hitPoints = value; playerControl.worldState.RegisterCharStat('g', value + ""); }
    } //g

    //attack attributes
    public EAttackMode attackMode{
        get { return attackMode; }
        set { attackMode = value; RegisterAttackMode(); }
    } //h
    [SerializeField]
    private float attackDamageBase;
    [SerializeField]
    private float attackAnimationTimeBase;
    [SerializeField]
    private float attackCooldownBase;

    public float range{
        get { return range; }
        set { range = value; playerControl.worldState.RegisterCharStat('i', value + ""); }
    } //i

    public float attackDamage {
        get { return attackDamage; }
        set { attackDamage = value; playerControl.worldState.RegisterCharStat('j', value + ""); } 
    } //j
    public float attackAnimationTime {
        get { return attackAnimationTime; }
        set { attackAnimationTime = value; playerControl.worldState.RegisterCharStat('k', value + ""); }
    } //k
    public float attackCooldown {
        get { return attackCooldown; }
        set { attackCooldown = value; playerControl.worldState.RegisterCharStat('l', value+ ""); }
    } //l

    //movement attributes
    [SerializeField]
    private float moveSpeedBase;
    [SerializeField]
    private float moveAccelerationBase;
    [SerializeField]
    private float moveSpeedMaxBase;

    public  float moveSpeed {
        get { return moveSpeed; }
        set { moveSpeed = value; playerControl.worldState.RegisterCharStat('m', value + ""); }
    } //m
    public float moveAcceleration {
        get { return moveAcceleration; }
        set { moveAcceleration = value; playerControl.worldState.RegisterCharStat('n', value + ""); }
    } //n
    public float moveSpeedMax {
        get { return moveSpeedMax; }
        set { moveSpeedMax = value; playerControl.worldState.RegisterCharStat('o', value + ""); }
    } //o

    //jump attributes
    [SerializeField]
    private float jumpSpeedBase;
    [SerializeField]
    private float jumpAccelerationBase;
    [SerializeField]
    private float jumpSpeedMaxBase;

    public float jumpSpeed {
        get { return jumpSpeed; }
        set { jumpSpeed = value; playerControl.worldState.RegisterCharStat('p', value + ""); }
    } //p
    public float jumpAcceleration {
        get { return jumpAcceleration; }
        set { jumpAcceleration = value; playerControl.worldState.RegisterCharStat('q', value + ""); }
    } //q
    public float jumpSpeedMax {
        get { return jumpSpeedMax; }
        set { jumpSpeedMax = value; playerControl.worldState.RegisterCharStat('r', value + ""); }
    } //r

    //gravity attributes
    [SerializeField]
    private float gravitySpeedBase;
    [SerializeField]
    private float gravityAccelerationBase;
    [SerializeField]
    private float gravitySpeedMaxBase;

    public float cayoteTime{
        get { return cayoteTime; }
        set { cayoteTime = value; playerControl.worldState.RegisterCharStat('s', value + ""); }
    } //s
    public float gravitySpeed {
        get { return gravitySpeed; }
        set { gravitySpeed = value; playerControl.worldState.RegisterCharStat('t', value + ""); }
    } //t
    public float gravityAcceleration {
        get { return gravityAcceleration; }
        set { gravityAcceleration = value; playerControl.worldState.RegisterCharStat('u', value + ""); }
    } //u
    public float gravitySpeedMax {
        get { return gravitySpeedMax; }
        set { gravitySpeedMax = value; playerControl.worldState.RegisterCharStat('v', value + ""); } 
    } //v

    // size attributes
    public Vector2 size { get; set; }
    // Use this for initialization
    void Start ()
    {
        playerControl = GetComponent<PlayerControl>();
        Initialize();
	}

    private void Initialize()
    {
        //States 
        feetState = EFeetState.Onground;
        //Hp
        hitPoints = hitPointsBase;
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

        gameObject.layer = LayerMask.NameToLayer(teamName);
        side = Vector2.right;
    }

    public void ResetGravitySpeed()
    {
        gravitySpeed = gravitySpeedBase;
    }
    public void ResetMoveSpeed()
    {
        moveSpeed = moveSpeedBase;
    }

    public void SetAttribute(char attributeCode, string value){
        switch(attributeCode){
            case 'a': SetHeadState(value); break;
            case 'b': SetBodyState(value); break;
            case 'c': SetHandState(value); break;
            case 'd': SetFeetState(value); break;
            case 'e': side = Toolkit.DeserializeVector(value); break;
            case 'f': armor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'g': hitPoints = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'h': SetAttackMode(value); break;
            case 'i': range = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'j': attackDamage = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'k': attackAnimationTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'l': attackCooldown = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'm': moveSpeed = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'n': moveAcceleration = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'o': moveSpeedMax  = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'p': jumpSpeed = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'q': jumpAcceleration = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'r': jumpSpeedMax = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 's': cayoteTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 't': gravitySpeed = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'u': gravityAcceleration = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'v': gravitySpeedMax = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
        }
    }
    private void SetHeadState(string value)
    {
        switch (value)
        {
            case "1": HeadState = EHeadState.Conscious; break;
            case "2": HeadState = EHeadState.Stunned; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetBodyState(string value){
        switch (value)
        {
            case "1": BodyState = EBodyState.Standing; break;
            case "2": bodyState = EBodyState.Moving; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetHandState(string value){
        switch (value)
        {
            case "1": HandState = EHandState.Idle; break;
            case "2": HandState = EHandState.Moving; break;
            case "3": HandState = EHandState.Attacking; break;
            case "4": HandState = EHandState.Casting; break;
            case "5": HandState = EHandState.Channeling; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetFeetState(string value){
        switch (value)
        {
            case "1": FeetState = EFeetState.Onground; break;
            case "2": FeetState = EFeetState.Falling; break;
            case "3": FeetState = EFeetState.Jumping; break;
            case "4": FeetState = EFeetState.NoGravity; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetAttackMode(string value){
        switch (value)
        {
            case "1": attackMode = EAttackMode.Ranged; break;
            case "2": attackMode = EAttackMode.Melee; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }

    private void RegisterHeadState(){
        switch(HeadState){
            case EHeadState.Conscious: playerControl.worldState.RegisterCharStat('a', "1"); break;
            case EHeadState.Stunned: playerControl.worldState.RegisterCharStat('a', "2"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }
    private void RegisterBodyState()
    {
        switch (BodyState)
        {
            case EBodyState.Standing: playerControl.worldState.RegisterCharStat('b', "1"); break;
            case EBodyState.Moving: playerControl.worldState.RegisterCharStat('b', "2"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }
    private void RegisterHandState()
    {
        switch (HandState)
        {
            case EHandState.Idle: playerControl.worldState.RegisterCharStat('c', "1"); break;
            case EHandState.Moving: playerControl.worldState.RegisterCharStat('c', "2"); break;
            case EHandState.Attacking: playerControl.worldState.RegisterCharStat('c', "3"); break;
            case EHandState.Casting: playerControl.worldState.RegisterCharStat('c', "4"); break;
            case EHandState.Channeling: playerControl.worldState.RegisterCharStat('c', "5"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }
    private void RegisterFeetState()
    {
        switch (FeetState)
        {
            case EFeetState.Onground: playerControl.worldState.RegisterCharStat('d', "1"); break;
            case EFeetState.Falling: playerControl.worldState.RegisterCharStat('d', "2"); break;
            case EFeetState.Jumping: playerControl.worldState.RegisterCharStat('d', "3"); break;
            case EFeetState.NoGravity: playerControl.worldState.RegisterCharStat('d', "4"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }

    private void RegisterAttackMode(){
        switch(attackMode){
            case EAttackMode.Ranged: playerControl.worldState.RegisterCharStat('h', "1"); break;
            case EAttackMode.Melee: playerControl.worldState.RegisterCharStat('h', "2"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }
}



public enum EHeadState { Conscious = 1, Stunned = 2 };
public enum EBodyState { Standing = 1,Moving = 2 };
public enum EHandState { Idle = 1, Moving = 2, Attacking = 3, Casting = 4, Channeling = 5 };
public enum EFeetState { Onground = 1, Falling = 2, Jumping = 3, NoGravity = 4 };
public enum EAttackMode { Ranged = 1, Melee = 2 };

