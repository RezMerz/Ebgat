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
    private bool axis;
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

        // Move left and Right
        if ((Input.GetAxis("Horizontal") > 0.3 || Input.GetAxis("Horizontal") < -0.3) && !axis)
        { 
            if (Input.GetAxis("Horizontal") > 0.1)
            {
                axis = true;
                clientNetworkSender.Move(1);
            }
            else if (Input.GetAxis("Horizontal") < -0.1)
            {
                axis = true;
                clientNetworkSender.Move(-1);
            }
        }
        else if (Input.GetAxis("Horizontal") < 0.3 && Input.GetAxis("Horizontal")> -0.3 && axis)
        {
            axis = false;
            clientNetworkSender.MoveFinished(transform.position);
        }
        if(Input.GetAxis("Vertical") != 0)
        {
            if(Input.GetAxis("Vertical") > 0.1)
            {

            }
            else if(Input.GetAxis("Vertical") < -0.1)
            {

            }
        }

        //Attack
        if (Input.GetButtonDown("Fire"))
        {
            Vector2 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            attack.AttackPressed(Input.mousePosition);
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
        else if (Input.GetButton("Jump"))
        {
            //jump.JumpHold();
        }

        if (Input.GetButtonUp("Jump"))
        {
            //clientNetworkSender.JumpReleased(transform.position);
            //jump.JumpReleased();
        }

        if (Input.GetButtonDown("Ability1"))
        {
            abilityManager.Ability1Pressed();
        }
        if (Input.GetButtonDown("Ability2"))
        {
            print("Ability 2");
        }
	}


}
