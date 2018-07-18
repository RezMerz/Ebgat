using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour
{
    public float speed;

    private PlayerControl playerControl;

    private Attack attack;
    private Moveable characterMove;
    private PlayerJump jump;
	// Use this for initialization
	void Start ()
    {
        playerControl = GetComponent<PlayerControl>();
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
        if (Input.GetKey(KeyCode.D))
        {
            playerControl.CmdMove(1);
            //characterMove.MovePressed(1);
        }
        if (Input.GetKey(KeyCode.A))
        {
            playerControl.CmdMove(-1);
            //characterMove.MovePressed(-1);
        }

        //move button released
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            playerControl.CmdMoveFinished(transform.position);
            Debug.Log("Local: Move Press Released");
        }

        //Attack
        if (Input.GetMouseButtonDown(0))
            attack.AttackPressed(Input.mousePosition);


        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerControl.CmdJump(transform.position);
            //jump.JumpPressed();
        }
	}


}
