using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HEllo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate(){
        Debug.Log(Time.deltaTime);
    }
}
