using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour
{
    public float speed;
    private PlayerControl playerControl;

    private CharacterAttributes charStats;
    private Attack attack;
    private CharacterMove characterMove;
    private PlayerJump jump;
    private ClientNetworkSender clientNetworkSender;
    private AbilityManager abilityManager;
    private bool axisXChanged;
    private bool axisYChanged;
    private float axisY;
    private float axisX;
	// Use this for initialization
	void Start ()
    {
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        clientNetworkSender = playerControl.clientNetworkSender;
        attack = playerControl.attack;
        characterMove = playerControl.characterMove;
        abilityManager = GetComponent<AbilityManager>();

        jump = playerControl.jump;
	}
	// Update is called once per frame
	void Update ()
    {
        if (!playerControl.IsLocalPlayer())
            return;

        axisX = Input.GetAxis("Horizontal");
        axisY = Input.GetAxis("Vertical");

        // Move left and Right
        if ((!axisXChanged && axisX > 0.3 || axisX < -0.3))
        { 
            if (axisX > 0.1)
            {
                axisXChanged = true;
                clientNetworkSender.Move(1);
            }
            else if (axisX < -0.1)
            {
                axisXChanged = true;
                clientNetworkSender.Move(-1);
            }
        }
        else if (axisXChanged && axisX < 0.3 && axisX > -0.3)
        {
            axisXChanged = false;
            clientNetworkSender.MoveFinished(transform.position);
        }
        if(axisY != 0)
        {
            if(axisY > 0.1)
            {
                clientNetworkSender.MoveVertical(1);
            }
            else if(axisY < -0.1)
            {
                clientNetworkSender.MoveVertical(-1);
            }
        }

        //Attack
        if (Input.GetButtonDown("Fire"))
        {
            attack.AttackPressed();
        }
        else if(Input.GetButtonUp("Fire"))
        {
            attack.AttackReleased();
        }
        else if(Input.GetAxis("Fire") > 0.1)
        {
            //print("RT");
        }

        // Jump

        if (Input.GetButtonDown("Jump"))
        {
            clientNetworkSender.JumpPressed(transform.position);
            //jump.JumpPressed();
        }
        else if (Input.GetButtonUp("Jump"))
        {
            clientNetworkSender.JumpReleased(transform.position);
            //jump.JumpReleased();
        }

        if (Input.GetButtonUp("Jump"))
        {
            clientNetworkSender.JumpReleased(transform.position);
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
        if (axisY < -0.1f)
        {
            clientNetworkSender.DropDownPressed();
            axisYChanged = true;
        }
        else if (axisYChanged && axisY == 0)
        {
            clientNetworkSender.DropDownReleased();
            axisYChanged = false;
        }
    }


}
