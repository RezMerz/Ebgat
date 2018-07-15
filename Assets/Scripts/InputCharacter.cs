using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour
{
    public float speed;

    private Moveable charcterMove;
	// Use this for initialization
	void Start ()
    {
        charcterMove = GetComponent<Moveable>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetAxis("Horizontal") != 0)
        {
            List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
            if(Input.GetAxis("Horizontal")>0)
            {
                charcterMove.CheckMove(Vector2.right, speed * Time.deltaTime, 256,out hitObjects);
                charcterMove.Move(Vector2.right, speed * Input.GetAxis("Horizontal") * Time.deltaTime, hitObjects);
            }
            else
            {
                charcterMove.CheckMove(Vector2.left, speed * Time.deltaTime, 256,out hitObjects);
                charcterMove.Move(Vector2.left, speed * -Input.GetAxis("Horizontal") * Time.deltaTime, hitObjects);
            }
        }
	}
}
