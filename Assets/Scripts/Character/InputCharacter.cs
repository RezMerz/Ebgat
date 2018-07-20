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
	// Use this for initialization
	void Start ()
    {
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        attack = playerControl.attack;
        characterMove = playerControl.characterMove;

        jump = playerControl.jump;
	}
	// Update is called once per frame
	void Update ()
    {
        if (!playerControl.isLocalPlayer)
            return;

        // Move left and Right
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A)){
            Debug.Log(Time.frameCount);
            characterMove.MovePressed(1);
            playerControl.CmdMove(1);
        }
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)){
            Debug.Log(Time.frameCount);
            characterMove.MovePressed(-1);
            playerControl.CmdMove(-1);
        }
        else
            charStats.BodyState = EBodyState.Standing;

        //move button released
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            Debug.Log(Time.frameCount);
            playerControl.CmdMoveFinished(transform.position);
        }

        //Attack
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Time.frameCount);
            attack.AttackPressed(Input.mousePosition);
        }

        // Jump

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(Time.frameCount);
            playerControl.CmdJumpPressed(transform.position);
            jump.JumpPressed();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log(Time.frameCount);
            //.CmdJumpHold(transform.position);
            jump.JumpHold();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log(Time.frameCount);
            playerControl.CmdJumpReleased(transform.position);
            jump.JumpReleased();
        }
	}

}
