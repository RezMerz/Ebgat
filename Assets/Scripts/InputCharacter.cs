using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour
{
    public float speed;

    private Moveable characterMove;
    private PlayerJump jump;
	// Use this for initialization
	void Start ()
    {
        characterMove = GetComponent<Moveable>();
        jump = GetComponent<PlayerJump>();
	}
	// Update is called once per frame
	void Update ()
    {
        // Move left and Right
        if (Input.GetAxis("Horizontal") != 0 && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)))
        {
            if(Input.GetAxis("Horizontal")>0)
            {
                characterMove.MovePressed(1);
            }
            else
            {
                characterMove.MovePressed(-1);
            }
        }


        // Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump.JumpPressed();
        }
	}


}
