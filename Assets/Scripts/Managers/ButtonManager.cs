using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ButtonManager : MonoBehaviour {
    
    public int id { get; set; }
    public Action<String> clickAction;
    public string ip;

	// Use this for initialization
	void Start () {
		
	}


    public void Clicked(){
        clickAction(ip);
    }

    public void SetData(Action<string> clickAction, int id, string ip){
        this.clickAction += clickAction;
        this.id = id;
        this.ip = ip;
    }
}
