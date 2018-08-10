using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class CharacterAttributesClient : MonoBehaviour {
        private PlayerControl playerControl;
    int a;
    int ID;

    public EHeadState HeadState
    {
        get { return headState;}

        set {  headState = value; }
        
    }
    private EHeadState headState; //a
    private EBodyState bodyState; //b
    public EBodyState BodyState
    {
        get { return bodyState; }
        set {  bodyState = value; }
    }
    
    private EHandState handState; //c
    public EHandState HandState
    {
        get { return handState; }
        set { handState = value;}
    }

    private EFeetState feetState; //d
    public EFeetState FeetState
    {
        get { return feetState; }
        set {  feetState = value;  }
    }



    public string teamName, enemyTeamName;
    public bool canDoubleJump;

    private Vector2 side;
    public Vector2 Side {
        get { return side; }
        set { side = value;  } 
    } //e

    //health attributes
    [SerializeField]
    private float hitPointsBase;
    private float armor;
    public float Armor{
        get { return armor; }
        set {armor = value; }
    } //f      
    public float hitPoints;
    public float HitPoints {
        get { return hitPoints; }
        set { hitPoints = value;  }
    } //g

    //attack attributes

    private EAttackMode attackMode;
    public EAttackMode AttackMode{
        get { return attackMode; }
        set {attackMode = value; }
    } //h
    [SerializeField]
    private float attackDamageBase;
    [SerializeField]
    private float attackAnimationTimeBase;
    [SerializeField]
    private float attackCooldownBase;
    private float range;
    public float Range{
        get { return range; }
        set { range = value;  }
    } //i

    private float attackDamage;
    public float AttackDamage {
        get { return attackDamage; }
        set {attackDamage = value;  } 
    } //j

    private float attackAnimationTime;
    public float AttackAnimationTime {
        get { return attackAnimationTime; }
        set { attackAnimationTime = value;  }
    } //k
    private float attackCooldown;
    public float AttackCooldown {
        get { return attackCooldown; }
        set {  attackCooldown = value; }
    } //l

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
        set {  moveSpeed = value;}
    } //m
    private float moveAcceleration; 
    public float MoveAcceleration {
        get { return moveAcceleration; }
        set { moveAcceleration = value; }
    } //n
    private float moveSpeedMax;
    public float MoveSpeedMax {
        get { return moveSpeedMax; }
        set {  moveSpeedMax = value; }
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
        set {  jumpSpeed = value;  }
    } //p

    private float jumpAcceleration;
    public float JumpAcceleration {
        get { return jumpAcceleration; }
        set { jumpAcceleration = value; }
    } //q
    private float jumpSpeedMax;
    public float JumpSpeedMax {
        get { return jumpSpeedMax; }
        set { jumpSpeedMax = value;  }
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

    private float cayoteTime;
    public float CayoteTime{
        get { return cayoteTime; }
        set {  cayoteTime = value; }
    } //s

    private float gravitySpeed;
    public float GravitySpeed {
        get { return gravitySpeed; }
        set {  gravitySpeed = value;  }
    } //t

    private float gravityAcceleration;
    public float GravityAcceleration {
        get { return gravityAcceleration; }
        set {  gravityAcceleration = value; }
    } //u

    private float gravitySpeedMax;
    public float GravitySpeedMax {
        get { return gravitySpeedMax; }
        set {gravitySpeedMax = value;  }
    } //v

    // size attributes
    public Vector2 size { get; set; }
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        ID = playerControl.clientNetworkSender.PlayerID;
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
        cayoteTime = baseCayoteTime;
        //Size
        size = GetComponent<BoxCollider2D>().size * transform.localScale;

        gameObject.layer = LayerMask.NameToLayer(teamName);
        side = Vector2.right;
    }
    public void SetAttribute(char attributeCode, string value)
    {
        switch (attributeCode)
        {
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
            case 'o': moveSpeedMax = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
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
    private void SetBodyState(string value)
    {
        switch (value)
        {
            case "1": BodyState = EBodyState.Standing; break;
            case "2": bodyState = EBodyState.Moving; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetHandState(string value)
    {
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
    private void SetFeetState(string value)
    {
        switch (value)
        {
            case "1": FeetState = EFeetState.Onground; break;
            case "2": FeetState = EFeetState.Falling; break;
            case "3": FeetState = EFeetState.Jumping; break;
            case "4": FeetState = EFeetState.NoGravity; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetAttackMode(string value)
    {
        switch (value)
        {
            case "1": attackMode = EAttackMode.Ranged; break;
            case "2": attackMode = EAttackMode.Melee; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
}
