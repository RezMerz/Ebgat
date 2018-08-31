using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonManager : MonoBehaviour {
    
    public int id { get; set; }
    public Action<int> clickAction;
    public string ip;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Clicked(){
        clickAction(id);
    }

    public void SetData(Action<int> clickAction, int id, string ip){
        this.clickAction += clickAction;
        this.id = id;
        this.ip = ip;
    }
}
