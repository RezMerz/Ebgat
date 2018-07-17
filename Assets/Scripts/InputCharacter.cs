using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour
{
    public float speed;

    private Attack attack;
    private Moveable characterMove;
    private PlayerJump jump;
	// Use this for initialization
	void Start ()
    {
        attack = GetComponent<Attack>();
        characterMove = GetComponent<Moveable>();
        jump = GetComponent<PlayerJump>();
	}
	// Update is called once per frame
	void Update ()
    {
        // Move left and Right
        if (Input.GetKey(KeyCode.D))
            characterMove.MovePressed(1);
        if (Input.GetKey(KeyCode.A))
            characterMove.MovePressed(-1);
        

        //Attack
        if (Input.GetMouseButtonDown(0))
            attack.AttackPressed(Input.mousePosition);


        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump.JumpPressed();
        }
	}


}
