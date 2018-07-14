using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Grab : MonoBehaviour {
    public float grab_range_;

    bool is_grabing_;
    Vector2 direction_;

    KeyCode grab_key_;
	
    // Use this for initialization
	void Start ()
    {
        grab_key_ = GetComponent<Player_Control>().grab_;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Grab();
		
	}
    void Grab()
    {
        if (!is_grabing_ && Input.GetKey(grab_key_))
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (GetComponent<Moveable>().Move_Up(grab_range_, true))
                {
                    is_grabing_ = true;
                    GetComponent<Player_Move>().Jump_off();
                    GetComponent<Gravity>().Deactivition();

                }
            }
            else
            {
                direction_ =GetComponent<Player_Move>().Get_Direction();
                if(direction_ == Vector2.right)
                {
                    if (GetComponent<Moveable>().Move_Right(grab_range_, true))
                    {
                        is_grabing_ = true;
                        GetComponent<Player_Move>().Jump_off();
                        GetComponent<Gravity>().Deactivition();
                    }
                }
                else
                {
                    if (GetComponent<Moveable>().Move_Left(grab_range_, true))
                    {
                        is_grabing_ = true;
                        GetComponent<Player_Move>().Jump_off();
                        GetComponent<Gravity>().Deactivition();
                    }
                }

            }
        }
        if(is_grabing_ && Input.GetKeyUp(grab_key_))
        {
            is_grabing_ = false;
            GetComponent<Gravity>().Activation();
        }
    }
}
