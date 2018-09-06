using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour
{
    private PlayerControlClientside playerControl;
    private ClientNetworkSender clientNetworkSender;
    private AbilityManager abilityManager;
    private bool axisXChanged;
    private bool axisYChanged;
    private bool newAxisChanged;
    private float axisY;
    private float axisX;
    private bool aiming;
    private CharacterAim aim;
    private bool attacking;
    public bool start { get;set; }

    private bool controllerConnected;
	// Use this for initialization
	void Start ()
    {
        playerControl = GetComponent<PlayerControlClientside>();
        playerControl.ReadyAction += Initialize;
        if (Input.GetJoystickNames().Length > 0)
        {
            if (Input.GetJoystickNames()[0].Length > 0)
                controllerConnected = true;
        }
	}
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!start || !playerControl.IsLocalPlayer())
            return;

        axisX = Input.GetAxis("Horizontal");
        axisY = Input.GetAxis("Vertical");
        // Move left and Right
        if ((!axisXChanged && (axisX > 0.3 || axisX < -0.3)))
        {
            axisXChanged = true;
            if (axisX > 0.1)
                clientNetworkSender.Move(1);
            else if (axisX < -0.1)
                clientNetworkSender.Move(-1);
        }
        else if (axisXChanged && axisX < 0.3 && axisX > -0.3)
        {
            axisXChanged = false;
            clientNetworkSender.MoveFinished();
        }

        // Move Down and Top
    /*    if ((!axisYChanged && axisY > 0.3 || axisY < -0.3))
        {
            axisYChanged = true;
            if(axisY > 0.1)
                clientNetworkSender.SetVerticalDirection(1);
            else if(axisY < -0.1)
                clientNetworkSender.SetVerticalDirection(-1);
        }
        else if (axisYChanged && axisY < 0.3 && axisY > -0.3)
        {
            axisYChanged = false;
            clientNetworkSender.SetVerticalDirection(0);
        } */

        //Attack
        if (Input.GetButtonDown("Fire") && !attacking)
        {
            attacking = true;
            clientNetworkSender.AttackPressed();
        }
        else if(Input.GetButtonUp("Fire") && attacking)
        {
            attacking = false;
            clientNetworkSender.AttackReleased();
        }
        else if(Input.GetAxis("Fire") > 0.1)
        {
            //print("RT");
        }

        // Jump

        if (Input.GetButtonDown("Jump"))
        {
            clientNetworkSender.JumpPressed();
            //jump.JumpPressed();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            clientNetworkSender.JumpReleased();
            //jump.JumpReleased();
        }

        if (Input.GetButtonUp("Jump"))
        {
            // clientNetworkSender.JumpReleased(transform.position);
            //jump.JumpReleased();
        }
        //Ability
        if (Input.GetButtonDown("Ability1"))
        {
            clientNetworkSender.Ability1Pressed();
        }
        if (Input.GetButtonDown("Ability2"))
        {
            clientNetworkSender.Ability2Pressed();
        }
        //Drop Down
        if (!newAxisChanged&& axisY < -0.1f && !aiming)
        {
            clientNetworkSender.DropDownPressed();
            newAxisChanged = true;
        }
        else if (newAxisChanged && axisY == 0)
        {
            clientNetworkSender.DropDownReleased();
            newAxisChanged = false;
        }

        if(Input.GetButtonDown("Dash"))
        {
            //print("Dash");
            clientNetworkSender.DashPressed();
        }
        if (Input.GetButtonDown("Aim"))
        {
            aiming = true;
            clientNetworkSender.AimPressed();
        }
        else if (Input.GetButtonUp("Aim"))
        {
            aiming = false;
            clientNetworkSender.AimReleased();
        }
        if (Input.GetAxis("Aim") > 0.5 && !aiming)
        {    
            aiming = true;
            clientNetworkSender.AimPressed();
        }
        else if (Input.GetAxis("Aim") < 0.2 && aiming)
        {
            aiming = false;
            clientNetworkSender.AimReleased();
        }

        if (Input.GetAxis("Fire") > 0.3  && !attacking)
        {
            attacking = true;
            print("get Axis");
            clientNetworkSender.AttackPressed();
        }
        else if (Input.GetAxis("Fire") < 0.3 && attacking)
        {
            attacking = false;
           // clientNetworkSender.AttackReleased();
        }
        if (aiming)
        {
            if (controllerConnected)
            {
                    clientNetworkSender.AimAxis(new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical")));
            }
            else
            {
                if (Input.GetAxis("Mouse Y") != 0)
                    clientNetworkSender.deltaY(Input.GetAxis("Mouse Y"));
                if (Input.GetAxis("Mouse X") != 0)
                    clientNetworkSender.deltaX(Input.GetAxis("Mouse X"));
            }
          
        }
        if (Input.GetButtonDown("Debug"))
        {
            clientNetworkSender.RequestWorldState(playerControl.playerId,playerControl.lastStateChecked);
        }

    }

    private void Initialize()
    {
        clientNetworkSender = playerControl.clientNetworkSender;
        abilityManager = GetComponent<AbilityManager>();

        start = true;
    }

}
