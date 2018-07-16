using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    private CharacterAttributes charStats;
	// Use this for initialization
	void Start () {
        charStats = GetComponent<CharacterAttributes>();
	}
}
