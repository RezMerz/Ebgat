using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCharacter : MonoBehaviour {
    public float speed;

    Moveable move;
	// Use this for initialization
	void Start () {
        move = GetComponent<Moveable>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("Horizontal") != 0)
        {
            if(Input.GetAxis("Horizontal")>0)
            {
                List<RaycastHit2D> hitObjects = move.CheckMove(Vector2.right, speed * Time.deltaTime, 256);
                move.Move(Vector2.right, speed * Time.deltaTime, hitObjects);
            }
            else
            {
                List<RaycastHit2D> hitObjects = move.CheckMove(Vector2.left, speed * Time.deltaTime, 256);
                move.Move(Vector2.left,speed * Time.deltaTime, hitObjects);
            }
        }
	}
}
