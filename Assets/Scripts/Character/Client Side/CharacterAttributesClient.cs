using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class CharacterAttributesClient : MonoBehaviour {
    
    private PlayerControl playerControl;
    public string teamName, enemyTeamName;
    public Vector2 side;
    
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerControl.ReadyAction += SetReady;
    }

    public void SetReady(){
      
    }

    public void SetAttribute(char attributeCode, string value)
    {
        switch (attributeCode)
        {
         //   case 'a': SetHeadState(value); break;
          //  case 'b': SetBodyState(value); break;
        //    case 'c': SetHandState(value); break;
       //     case 'd': SetFeetState(value); break;
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
        //    case 'w': aimSide = Toolkit.DeserializeVector(value); break;
        //    case 'x': energy = int.Parse(value); break;
        //    case 'A': SetAbilityState(value); break;
        }
    }
}
