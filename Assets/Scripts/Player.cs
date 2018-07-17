using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public static GameObject main_player_;
    private string state_;

	// Use this for initialization
	void Start () {
        if(main_player_ == null)
        {
            main_player_ = this.gameObject;
        }
        else
        {
            Destroy(gameObject);
        }
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Set_State(string state)
    {
        state_ = state;
    }
    public string Get_State()
    {
        return state_;
    }
}
