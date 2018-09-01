using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Globalization;
public class CharacterAttributes : MonoBehaviour {

    private PlayerControl playerControl;
    int ID;


    private EAbility abilityState;
    public EAbility AbilityState
    {
        get { return abilityState; }
        set { abilityState = value;
            if (value == EAbility.Ability1Start)
                Energy -= ability1EnergyConsume;
            else if (value == EAbility.Ability2Start)
                Energy -= ability2EnergyConsume;
            RegisterAbilityState(); 
        }
    }

    private EHeadState headState; //a
    public EHeadState HeadState
    {
        get { return headState;}

        set { if (value != headState) { headState = value; RegisterHeadState(); } }
        
    }

    private EBodyState bodyState; //b
    public EBodyState BodyState
    {
        get { return bodyState; }
        set { if (value != bodyState) { 
            if (value == EBodyState.Dashing) Energy -= dashEnergyConsume; 
            bodyState = value; RegisterBodyState(); } }
    }
    
    private EHandState handState; //c
    public EHandState HandState
    {
        get { return handState; }
        set { if (value != handState)
         { 
            handState = value;
            if (value == EHandState.Attacking)
                Energy -= attackEnergyConsume;
            RegisterHandState(); 
         } 
        }
    }

    private EFeetState feetState; //d
    public EFeetState FeetState
    {
        get { return feetState; }
        set { 
            if (value != feetState)
            {
              
              feetState = value;
              RegisterFeetState(); 
            } 
        }
    }



    public string teamName, enemyTeamName;
    public bool canDoubleJump;

    private Vector2 side;
    
    public Vector2 Side {
        get { return side; }
        set { if (value != side) { side = value; playerControl.worldState.RegisterCharStat(ID, 'e', Toolkit.VectorSerialize(value)); } } 
    } //e

    public int wallside;

    //health attributes
    [SerializeField]
    private float hitPointsBase;
    private float armor;
    public float Armor{
        get { return armor; }
        set {if(value != armor) { armor = value; playerControl.worldState.RegisterCharStat(ID, 'f', value + ""); }}
    } //f 
     
    private float hitPoints;
    public float HitPoints {
        get { return hitPoints; }
        set { if (value != hitPoints) { hitPoints = value; playerControl.worldState.RegisterCharStat(ID, 'g', value + ""); } }
    } //g

    //attack attributes

    private EAttackMode attackMode;
    public EAttackMode AttackMode{
        get { return attackMode; }
        set { if (value != attackMode) { attackMode = value; RegisterAttackMode(); } }
    } //h
    [SerializeField]
    private float attackDamageBase;
    [SerializeField]
    private float attackAnimationTimeBase;
    [SerializeField]
    private float attackCooldownBase;
    [SerializeField]
    private int attackNumber;

    private float range;
    public float Range{
        get { return range; }
        set { if (value != range) { range = value; playerControl.worldState.RegisterCharStat(ID, 'i', value + ""); } }
    } //i

    private float attackDamage;
    public float AttackDamage {
        get { return attackDamage; }
        set { if (value != attackDamage) { attackDamage = value; playerControl.worldState.RegisterCharStat(ID, 'j', value + ""); } } 
    } //j

    private float attackAnimationTime;
    public float AttackAnimationTime {
        get { return attackAnimationTime; }
        set { if (value != attackAnimationTime) { attackAnimationTime = value; playerControl.worldState.RegisterCharStat(ID, 'k', value + ""); } }
    } //k
    private float attackCooldown;
    public float AttackCooldown {
        get { return attackCooldown; }
        set { if (value != attackCooldown) { attackCooldown = value; playerControl.worldState.RegisterCharStat(ID, 'l', value + ""); } }
    } //l

    public int AttackNumber
    {
        get { return attackNumber; }
        set { if(value != attackNumber) { attackNumber = value ;playerControl.worldState.RegisterCharStat(ID, 'y', value + ""); } }
    }

    //movement attributes
    [SerializeField]
    private float moveSpeedBase;
    [SerializeField]
    private float moveAccelerationBase;
    [SerializeField]
    private float moveSpeedMaxBase;


    private float moveSpeed;
    public  float MoveSpeed {
        get { return moveSpeed; }
        set { if (value != moveSpeed) { moveSpeed = value; playerControl.worldState.RegisterCharStat(ID, 'm', value + ""); } }
    } //m
    private float moveAcceleration; 
    public float MoveAcceleration {
        get { return moveAcceleration; }
        set { if (value != moveAcceleration) { moveAcceleration = value; playerControl.worldState.RegisterCharStat(ID, 'n', value + ""); } }
    } //n
    private float moveSpeedMax;
    public float MoveSpeedMax {
        get { return moveSpeedMax; }
        set { if (value != moveSpeedMax) { moveSpeedMax = value; playerControl.worldState.RegisterCharStat(ID, 'o', value + ""); } }
    } //o

    //jump attributes
    [SerializeField]
    private float jumpSpeedBase;
    [SerializeField]
    private float jumpAccelerationBase;
    [SerializeField]
    private float jumpSpeedMaxBase;

    private float jumpSpeed;
    public float JumpSpeed {
        get { return jumpSpeed; }
        set { if (value != jumpSpeed) { jumpSpeed = value; playerControl.worldState.RegisterCharStat(ID, 'p', value + ""); } }
    } //p

    private float jumpAcceleration;
    public float JumpAcceleration {
        get { return jumpAcceleration; }
        set { if (value != jumpAcceleration) { jumpAcceleration = value; playerControl.worldState.RegisterCharStat(ID, 'q', value + ""); } }
    } //q
    private float jumpSpeedMax;
    public float JumpSpeedMax {
        get { return jumpSpeedMax; }
        set { if (value != jumpSpeedMax) { jumpSpeedMax = value; playerControl.worldState.RegisterCharStat(ID, 'r', value + ""); } }
    } //r





    //gravity attributes
    [SerializeField]
    private float gravitySpeedBase;
    [SerializeField]
    private float gravityAccelerationBase;
    [SerializeField]
    private float gravitySpeedMaxBase;
    [SerializeField]
    private float baseCayoteTime;
    [SerializeField]
    private float onWallGravitySpeed;

    private float cayoteTime;
    public float CayoteTime{
        get { return cayoteTime; }
        set { if (value != cayoteTime) { cayoteTime = value; playerControl.worldState.RegisterCharStat(ID, 's', value + ""); } }
    } //s

    private float gravitySpeed;
    public float GravitySpeed {
        get { return gravitySpeed; }
        set { if (value != gravitySpeed) { gravitySpeed = value; playerControl.worldState.RegisterCharStat(ID, 't', value + ""); } }
    } //t

    private float gravityAcceleration;
    public float GravityAcceleration {
        get { return gravityAcceleration; }
        set { if (value != jumpAcceleration) { gravityAcceleration = value; playerControl.worldState.RegisterCharStat(ID, 'u', value + ""); } }
    } //u

    private float gravitySpeedMax;
    public float GravitySpeedMax {
        get { return gravitySpeedMax; }
        set { if (value != jumpAcceleration) { gravitySpeedMax = value; playerControl.worldState.RegisterCharStat(ID, 'v', value + ""); } }
    } //v

    private Vector2 aimSide;
    public Vector2 AimSide
    {
        get { return aimSide; }
        set { if (value != aimSide) { aimSide = value; playerControl.worldState.RegisterCharStat(ID, 'w', Toolkit.VectorSerialize(value)); } }
    } //w

    /// Energy Attributes

    
    public int energyBase;

    private int energy;
    public int Energy
    {
        get { return energy; }
        set { if (value != energy) { energy = value; playerControl.worldState.RegisterCharStat(ID, 'x', value + ""); } }
    } //x


    [SerializeField]
    private float speedRate;

    public float SpeedRate
    {
        get { return speedRate; }
        set { if (value != speedRate) { speedRate = value ; playerControl.worldState.RegisterCharStat(ID, 'z', value + ""); } }
    }//z


    public int energyRegenRate;
    public int attackEnergyConsume;
    public int jumpEnergyConsume;
    public int ability1EnergyConsume;
    public int ability2EnergyConsume;
    public int dashEnergyConsume;

    // size attributes
    public Vector2 size { get; set; }

    private void Update()
    {
        ID = playerControl.playerId;
    }

    // Use this for initialization
    void Start ()
    {
        playerControl = GetComponent<PlayerControl>();
        ID = playerControl.playerId;
        playerControl.ReadyAction += SetReady;
	}

    public void SetReady(){
        Initialize();
    }

    private void Initialize()
    {
        //States 
        FeetState = EFeetState.Onground;
        HeadState = EHeadState.Conscious;
        BodyState = EBodyState.Standing;
        HandState = EHandState.Idle;
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
        //gravitySpeed = gravitySpeedBase;
        gravityAcceleration = gravityAccelerationBase;
        gravitySpeedMax = gravitySpeedMaxBase;
        cayoteTime = baseCayoteTime;
        //Size
        size = GetComponent<BoxCollider2D>().size * transform.localScale;

        gameObject.layer = LayerMask.NameToLayer(teamName);
        side = Vector2.right;

        // Energy
        energy = energyBase;

        //SpeedRate
        speedRate = 1;
    }

    public void ResetGravitySpeed()
    {
        gravitySpeed = gravitySpeedBase;
    }
    public void GravityOnWall()
    {
        GravitySpeed = onWallGravitySpeed;
    }

    public void ResetCayoteTime()
    {
        cayoteTime = baseCayoteTime;
    }
    public void ResetMoveSpeed()
    {
        moveSpeed = moveSpeedBase;
    }

    public void ResetJumpSpeed()
    {
        jumpSpeed = jumpSpeedBase;
    }
    public void RestJumpMaxSpeed()
    {
        JumpSpeedMax = jumpSpeedMaxBase;
    }

    public void ResetHP()
    {
        HitPoints = hitPointsBase;
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
            case 'w': aimSide = Toolkit.DeserializeVector(value); break;
            case 'x': energy = int.Parse(value); break;
            case 'A': SetAbilityState(value); break;
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
            case "3": bodyState = EBodyState.Dashing; break;
            case "4": bodyState = EBodyState.Aiming; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetHandState(string value){
        switch (value)
        {
            case "1": HandState = EHandState.Idle; break;
            case "2": HandState = EHandState.Attacking; break;
            case "3": HandState = EHandState.Casting; break;
            case "4": HandState = EHandState.Channeling; break;
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

    private void SetAbilityState(string value)
    {
        switch (value)
        {
            case "1": AbilityState = EAbility.Ability1Start; break;
            case "2": AbilityState = EAbility.Ability1Finish; break;
            case "3": AbilityState = EAbility.Ability2Start; break;
            case "4": AbilityState = EAbility.Ability2Finish; break;
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
            case EHeadState.Conscious: playerControl.worldState.RegisterCharStat(ID, 'a', "1"); break;
            case EHeadState.Stunned: playerControl.worldState.RegisterCharStat(ID, 'a', "2"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }
    private void RegisterBodyState()
    {
        switch (BodyState)
        {
            case EBodyState.Standing: playerControl.worldState.RegisterCharStat(ID, 'b', "1"); break;
            case EBodyState.Moving: playerControl.worldState.RegisterCharStat(ID, 'b', "2"); break;
            case EBodyState.Dashing: playerControl.worldState.RegisterCharStat(ID, 'b', "3"); break;
            case EBodyState.Aiming: playerControl.worldState.RegisterCharStat(ID, 'b', "4"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }
    private void RegisterHandState()
    {
        switch (HandState)
        {
            case EHandState.Idle: playerControl.worldState.RegisterCharStat(ID, 'c', "1"); break;
            case EHandState.Attacking: playerControl.worldState.RegisterCharStat(ID, 'c', "2"); break;
            case EHandState.Casting: playerControl.worldState.RegisterCharStat(ID, 'c', "3"); break;
            case EHandState.Channeling: playerControl.worldState.RegisterCharStat(ID, 'c', "4"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }
    private void RegisterFeetState()
    {
        switch (FeetState)
        {
            case EFeetState.Onground: playerControl.worldState.RegisterCharStat(ID, 'd', "1"); break;
            case EFeetState.Falling: playerControl.worldState.RegisterCharStat(ID, 'd', "2"); break;
            case EFeetState.Jumping: playerControl.worldState.RegisterCharStat(ID, 'd', "3"); break;
            case EFeetState.NoGravity: playerControl.worldState.RegisterCharStat(ID, 'd', "4"); break;
            case EFeetState.DoubleJumping: playerControl.worldState.RegisterCharStat(ID, 'd', "5"); break;
            case EFeetState.OnWall:playerControl.worldState.RegisterCharStat(ID, 'd', "6");break;
            case EFeetState.WallJumping: playerControl.worldState.RegisterCharStat(ID, 'd', "8"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    }

    private void RegisterAttackMode(){
        switch(attackMode){
            case EAttackMode.Ranged: playerControl.worldState.RegisterCharStat(ID, 'h', "1"); break;
            case EAttackMode.Melee: playerControl.worldState.RegisterCharStat(ID, 'h', "2"); break;
            default: UnityEngine.Debug.Log("error in registering"); break;
        }
    
    }
    private void RegisterAbilityState()
    {
        switch(abilityState)
        {
            case EAbility.Ability1Start: playerControl.worldState.RegisterCharStat(ID, 'A', "1"); break;
            case EAbility.Ability1Finish: playerControl.worldState.RegisterCharStat(ID, 'A', "2"); break;
            case EAbility.Ability2Start: playerControl.worldState.RegisterCharStat(ID, 'A', "3"); break;
            case EAbility.Ability2Finish: playerControl.worldState.RegisterCharStat(ID, 'A', "4"); break;
        }
    }

    public void RegisterAllStates(){
        print("Register All States");
        int id = playerControl.playerId;
        string data = "";
        RegisterHeadState();
        RegisterBodyState();
        RegisterHandState();
        RegisterFeetState();
        data += 'e' + "&" + Toolkit.VectorSerialize(Side) + "$";
        data += 'f' + "&" + Armor + "$";
        data += 'g' + "&" + HitPoints + "$";
        data += 'i' + "&" + Range + "$";
        data += 'j' + "&" + AttackDamage + "$";
        data += 'k' + "&" + AttackAnimationTime + "$";
        data += 'l' + "&" + AttackCooldown + "$";
        data += 'm' + "&" + MoveSpeed + "$";
        data += 'n' + "&" + MoveAcceleration + "$";
        data += 'o' + "&" + MoveSpeedMax + "$";
        data += 'p' + "&" + JumpSpeed + "$";
        data += 'q' + "&" + jumpAcceleration + "$";
        data += 'r' + "&" + JumpSpeedMax + "$";
        data += 's' + "&" + CayoteTime + "$";
        data += 't' + "&" + GravitySpeed + "$";
        data += 'u' + "&" + GravityAcceleration + "$";
        data += 'v' + "&" + GravitySpeedMax + "$";
        data += 'w' + "&" + Toolkit.VectorSerialize(AimSide) + "$";
        data += 'x' + "&" + energy + "&";
        playerControl.worldState.AppendCharstats(id, data);
    }
}



public enum EHeadState { Conscious = 1, Stunned = 2 };
public enum EBodyState { Standing = 1,Moving = 2 , Dashing = 3, Aiming = 4 };
public enum EHandState { Idle = 1, Attacking = 2, Casting = 3, Channeling = 4};
public enum EFeetState { Onground = 1, Falling = 2, Jumping = 3, NoGravity = 4 , DoubleJumping = 5,OnWall = 6,WallJumping = 8};
public enum EAttackMode { Ranged = 1, Melee = 2 };


public enum EAbility { Ability1Start = 1, Ability1Finish = 2, Ability2Start=3,Ability2Finish =4};

