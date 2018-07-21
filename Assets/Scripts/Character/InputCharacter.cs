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
	// Use this for initialization
	void Start ()
    {
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        clientNetworkSender = playerControl.clientNetworkSender;
        attack = playerControl.attack;
        characterMove = playerControl.characterMove;

        jump = playerControl.jump;
	}
	// Update is called once per frame
	void Update ()
    {
        if (!playerControl.IsLocalPlayer())
            return;

        // Move left and Right
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)){
            characterMove.MovePressed(1);
            clientNetworkSender.Move(1);
        }
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
            characterMove.MovePressed(-1);
            clientNetworkSender.Move(-1);
        }
        else
            charStats.BodyState = EBodyState.Standing;

        //move button released
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            clientNetworkSender.MoveFinished(transform.position);
        }

        //Attack
        if (Input.GetMouseButtonDown(0))
        {
            attack.AttackPressed(Input.mousePosition);
        }

        // Jump

        if (Input.GetKeyDown(KeyCode.Space))
        {
            clientNetworkSender.JumpPressed(transform.position);
            jump.JumpPressed();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            clientNetworkSender.JumpHold(transform.position);
            jump.JumpHold();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            clientNetworkSender.JumpReleased(transform.position);
            jump.JumpReleased();
        }
	}

}
