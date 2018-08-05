using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineInputCharacter : MonoBehaviour {

    private OfflineCharacterMove charMoveOff;
    private OfflineCharacterJump charJumpOff;
    private OfflineCharacterGravity charGravityOff;

    private float axisX;
    private float axisY;

    private bool axisYChanged;

	// Use this for initialization
	void Start ()
    {
        charMoveOff = GetComponent<OfflineCharacterMove>();
        charJumpOff = GetComponent<OfflineCharacterJump>();
        charGravityOff = GetComponent<OfflineCharacterGravity>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        axisX = Input.GetAxis("Horizontal");
        axisY = Input.GetAxis("Vertical");
        if (Mathf.Abs(axisX) > 0.3f)
        {
            if (axisX > 0)
                charMoveOff.MovePressed(1);
            else
                charMoveOff.MovePressed(-1);
        }
        else if (Mathf.Abs(axisX) < 0.3f && Mathf.Abs(axisX) > 0f)
        {
            charMoveOff.MoveReleased();
        }



        if(axisY < -0.1f)
        {
            charGravityOff.ExcludeBridge();
            axisYChanged = true;
        }
        else if (axisYChanged && axisY == 0)
        {
            charGravityOff.IncludeBridge();
            axisYChanged = false;
        }



        if (Input.GetKeyDown(KeyCode.Space))
        {
            charJumpOff.JumpPressed();
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            charJumpOff.JumpHold();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            charJumpOff.JumpReleased();
        }
        
    }
}
