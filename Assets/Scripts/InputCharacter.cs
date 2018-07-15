using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour
{
    public float speed;

    private Moveable characterMove;
	// Use this for initialization
	void Start ()
    {
        characterMove = GetComponent<Moveable>();
	}
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("Horizontal") != 0)
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
	}


}
