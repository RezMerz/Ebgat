using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Orb_Shoot : MonoBehaviour {
    GameObject first_orb_;
    Vector2 shift_position_;
    Vector3 direction_;

    KeyCode orb_shoot_key_;
    KeyCode orb_shift_key_;


    // Use this for initialization
    void Start ()
    {
        first_orb_ = Orb.first_orb_;
        orb_shoot_key_ = GetComponent<Player_Control>().orb_shoot_;
        orb_shift_key_ = GetComponent<Player_Control>().orb_shift_;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Orb_Shoot();
        Orb_Shift();
	}
    void Orb_Shoot()
    {
        direction_ = GetComponent<Player_Move>().Get_Direction();
        if (Input.GetKey(KeyCode.RightArrow))
        {
            direction_ = Vector2.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            direction_ = Vector2.left;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction_ = Vector2.up;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            direction_ = Vector2.down;
        }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            direction_ = new Vector2(1,1).normalized;
        }
        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            direction_ = new Vector2(1,-1).normalized;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.UpArrow))
        {
            direction_ = new Vector2(-1,1).normalized;
        }
        if (Input.GetKey(KeyCode.LeftArrow) && Input.GetKey(KeyCode.DownArrow))
        {
            direction_ = new Vector2(-1,-1).normalized;
        }
        if (Input.GetKey(orb_shoot_key_)) 
        {
            first_orb_.GetComponent<Orb>().Released(direction_);
            first_orb_.transform.position = transform.position + Vector3.up * first_orb_.GetComponent<Moveable_Circle>().radius_;
        }

    }
    void Orb_Shift()
    {
        Orb.State orb_state_ = first_orb_.GetComponent<Orb>().Get_State();

        if (Input.GetKey(orb_shift_key_) && orb_state_ != Orb.State.Hold)
        {
            first_orb_.GetComponent<Orb>().Change_Speed(Orb.Speed_State.Shift);
        }
        if (Input.GetKeyUp(orb_shift_key_) && orb_state_ != Orb.State.Hold)
        {
            first_orb_.GetComponent<Orb>().Change_Speed(Orb.Speed_State.Base);
            first_orb_.GetComponent<Orb>().Set_State(Orb.State.Hold);
            if(first_orb_.GetComponent<Orb>().Get_Shift_Position(out shift_position_))
            {
                transform.position = shift_position_;
            }
            GetComponent<Gravity>().Speed_Reset();

        }
    }
}
