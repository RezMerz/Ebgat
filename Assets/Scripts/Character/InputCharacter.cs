using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour
{
    private CharacterAttributes charStats;
    private Attack attack;
    private Moveable characterMove;
    private PlayerJump jump;
	// Use this for initialization
	void Start ()
    {
        charStats = GetComponent<CharacterAttributes>();
        attack = GetComponent<Attack>();
        characterMove = GetComponent<Moveable>();
        jump = GetComponent<PlayerJump>();
	}
	// Update is called once per frame
	void Update ()
    {
        // Move left and Right
        if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
            characterMove.MovePressed(1);
        else if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            characterMove.MovePressed(-1);
        else
            charStats.BodyState = EBodyState.Standing;

        //Attack
        if (Input.GetMouseButtonDown(0))
            attack.AttackPressed(Input.mousePosition);


        // Jump
        if (Input.GetKey(KeyCode.Space))
        {
            jump.JumpPressed();
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            jump.JumpReleased();
        }
	}


}
