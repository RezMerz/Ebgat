using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour {
    public float movement_speed_;
    public float max_movement_speed_;
    public float movement_acceleration_;
    public float jump_speed_;
    public float max_jump_speed;
    public float jump_acceleration_;
    float right_movement_timer_ = 0;
    float left_movement_timer_ = 0;
    float jump_timer_ = 0;
    float movement_distance_;
    float jump_distance_;
    bool on_ground_;
    bool on_jump_ = false;
    bool on_wall_ = false;
    bool jump_command_ = false;
    bool jump_hold_ = true;
    Vector2 direction = Vector2.right;
    KeyCode jump_key_;
    
	// Use this for initialization
	void Start ()
    {
        jump_key_ = GetComponent<Player_Control>().jump_;
	}
	
	// Update is called once per frame
	void Update ()
    {
        On_Ground_Check();
        Jump();
        Move();
	}
    void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            direction = Vector2.right;
            on_wall_ = false;
            if (on_ground_)
            {
                right_movement_timer_ += Time.deltaTime;
            }
            float movement_hold_speed_ = right_movement_timer_ * movement_acceleration_;
            if(movement_hold_speed_ > max_movement_speed_ - movement_speed_)
            {
                movement_hold_speed_ = max_movement_speed_ - movement_speed_;
            }
            movement_distance_ = (movement_speed_ + movement_hold_speed_)* Time.deltaTime;
            if (GetComponent<Moveable>().Move_Right(movement_distance_,false) && !on_jump_)
            {
                right_movement_timer_ = 0;
                on_wall_ = true ;
                GetComponent<Gravity>().Change_Properties(0.3f, 5);
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
        {
            direction = Vector2.left;
            on_wall_ = false;
            if (on_ground_)
            {
                left_movement_timer_ += Time.deltaTime;
            }
            float movement_hold_speed_ = left_movement_timer_ * movement_acceleration_;
            if (movement_hold_speed_ > max_movement_speed_ - movement_speed_)
            {
                movement_hold_speed_ = max_movement_speed_ - movement_speed_;
            }
            movement_distance_ = (movement_speed_ + movement_hold_speed_) * Time.deltaTime;
            if (GetComponent<Moveable>().Move_Left(movement_distance_,false) && !on_jump_)
            {
                left_movement_timer_ = 0;
                on_wall_ = true;
                GetComponent<Gravity>().Change_Properties(0.3f, 7.5f);
            }
        }
        else
        {
            right_movement_timer_ = 0;
            left_movement_timer_ = 0;
            on_wall_ = false;
        }
        if (!on_wall_)
        {
            GetComponent<Gravity>().Change_Properties(-1, -1);
        }
    }
    void Jump()
    {
        
        if (on_ground_)
        {
            if (!Input.GetKey(jump_key_))
            {
                jump_hold_ = true;
                jump_command_ = true;
                jump_timer_ = 0;
            }
        }
        else
        {
            jump_command_ = false;
        }
        if (Input.GetKey(jump_key_) && jump_hold_)
        {
            jump_timer_ += Time.deltaTime;
            if (jump_command_)
            {
                on_jump_ = true;
                jump_command_ = false;
            }
        }
        if (Input.GetKeyUp(jump_key_))
        {
            jump_hold_ = false;
        }
        float jump_hold_speed_ = jump_timer_ * jump_acceleration_;
        if(jump_hold_speed_ > max_jump_speed - jump_speed_)
        {
            jump_hold_speed_ = max_jump_speed - jump_speed_;
        }
        jump_distance_ = (jump_speed_+jump_hold_speed_) * Time.deltaTime;

        if (on_jump_)
        {
            float gravity_distance_ = GetComponent<Gravity>().Get_Distance();
            GetComponent<Gravity>().Deactivition();
            if ( GetComponent<Moveable>().Move_Up(jump_distance_-gravity_distance_,false) || jump_distance_ <= gravity_distance_)
            {
                on_jump_ = false;
                GetComponent<Gravity>().Activation();
            }
        }
    }
    void On_Ground_Check()
    {
        bool pre_on_ground = on_ground_;
        on_ground_ = GetComponent<Gravity>().on_ground_;
        if (on_ground_)
        {
            GetComponent<Gravity>().Change_Properties(-1, -1);
            if (pre_on_ground == false)
            {
                right_movement_timer_ = 0;
                left_movement_timer_ = 0;
            }
        }
    }
    public Vector2 Get_Direction()
    {
        return direction;
    }
    public void Jump_off()
    {
        on_jump_ = false;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
    }
    
}
