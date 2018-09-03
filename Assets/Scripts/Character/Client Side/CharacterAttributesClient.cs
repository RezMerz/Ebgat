using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class CharacterAttributesClient : MonoBehaviour {
    
    private PlayerControlClientside playerControl;
    public string teamName{get;set;}
    public string enemyTeamName { get;set;}
    public Vector2 side { get; set; }
    public Vector2 aimSide { get; set; }

    public EHeadState headState { get; set; }
    public EHandState handState { get; set; }
    public EBodyState bodyState { get; set; }

    public EFeetState feetState { get; set; }
    public float aimRotation { get; set; }

    public float hp { get; set; }
    public int energy { get; set; }

    public float hpBase;
    public int energyBase;
    void Start()
    {
        playerControl = GetComponent<PlayerControlClientside>();
        playerControl.ReadyAction += SetReady;
        hpBase = playerControl.charStatsClient.hpBase;
        energyBase = playerControl.charStatsClient.energyBase;
    }

    public void SetReady(){
        Initialize();
    }
    private void Initialize()
    {
        //States 
        feetState = EFeetState.Onground;
        headState = EHeadState.Conscious;
        bodyState = EBodyState.Standing;
        handState = EHandState.Idle;

        hpBase = playerControl.charStatsClient.hpBase;
        energyBase = playerControl.charStatsClient.energyBase;
        //Hp
        hp = hpBase;
        energy = energyBase;
      
    }

      private void SetHeadState(string value)
    {
        switch (value)
        {
            case "1": headState = EHeadState.Conscious; break;
            case "2": headState = EHeadState.Stunned; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetBodyState(string value){
        switch (value)
        {
            case "1": bodyState = EBodyState.Standing; break;
            case "2": bodyState = EBodyState.Moving; break;
            case "3": bodyState = EBodyState.Dashing; break;
            case "4": bodyState = EBodyState.Aiming; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetHandState(string value){
        switch (value)
        {
            case "1": handState = EHandState.Idle; break;
            case "2": handState = EHandState.Attacking; break;
            case "3": handState = EHandState.Casting; break;
            case "4": handState = EHandState.Channeling; break;
            case "5": handState = EHandState.Disarm; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
    }
    private void SetFeetState(string value)
    {
        switch (value)
        {
            case "1": feetState = EFeetState.Onground; break;
            case "2": feetState = EFeetState.Falling; break;
            case "3": feetState = EFeetState.Jumping; break;
            case "4": feetState = EFeetState.NoGravity; break;
            case "5": feetState = EFeetState.DoubleJumping; break;
            case "6": feetState = EFeetState.OnWall;break;
            case "8": feetState = EFeetState.WallJumping;break;
            case "9": feetState = EFeetState.Root; break;
            default: UnityEngine.Debug.Log("Error is setting char stat"); break;
        }
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
        //    case 'f': armor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'g': hitPoints = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'h': SetAttackMode(value); break;
       //     case 'i': range = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'j': attackDamage = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'k': attackAnimationTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
      //      case 'l': attackCooldown = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
      //      case 'm': moveSpeed = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'n': moveAcceleration = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
        //    case 'o': moveSpeedMax = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'p': jumpSpeed = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'q': jumpAcceleration = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'r': jumpSpeedMax = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 's': cayoteTime = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 't': gravitySpeed = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
       //     case 'u': gravityAcceleration = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
        //    case 'v': gravitySpeedMax = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat); break;
            case 'w': aimSide = Toolkit.DeserializeVector(value); break;
        //    case 'x': energy = int.Parse(value); break;
        //    case 'A': SetAbilityState(value); break;
            case 'C': aimRotation = float.Parse(value); break;
        }
    }
}
