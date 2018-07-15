using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour {
    CharacterAttributes charStats;
	// Use this for initialization
	void Start () {
        charStats = GetComponent<CharacterAttributes>();
	}

    public void JumpPressed()
    {
        // Jump only if on ground
        if (charStats.FeetState == EFeetState.Onground)
        {
            /// Jump Code
            print("Jump");
        }
    }
}
