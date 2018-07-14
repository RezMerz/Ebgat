using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour {
    public enum State {Hold,Released,Hit};
    public enum Speed_State {Base,Shift};

    public float base_speed_;
    public float shift_speed_;
    public static GameObject first_orb_;

    private State state_;
    private float speed_;
    private Vector2 released_direction_;
    private bool shiftable_;

    private void Awake()
    {
        first_orb_ = this.gameObject;
        state_ = State.Hold;
        speed_ = base_speed_;
        shiftable_ = true;
    }
	private void Update ()
    {
        Position_Set();
        Shoot();
	}
    private void Position_Set()
    {
        if (state_ == State.Hold)
        {
            transform.position = Player.main_player_.transform.position;
        }
    }
    private void Shoot()
    {
        if (state_ == State.Released)
        {
            GetComponentInChildren<TrailRenderer>().enabled = true;
            float distance = speed_* Time.deltaTime;
            if(GetComponent<Moveable_Circle>().Move(distance, released_direction_))
            {
                state_ = State.Hit;
                GetComponentInChildren<TrailRenderer>().enabled = false;
            }
        }
    }
    public void Released(Vector2 direction)
    {
        state_ = State.Released;
        released_direction_ = direction;
        shiftable_ = true;
    }
    public void Set_State(State state_)
    {
        this.state_ = state_;
        if (this.state_ == State.Hold)
        {
            GetComponentInChildren<TrailRenderer>().enabled = false;
        }
    }
    public void Change_Speed(Speed_State speed_state_)
    {
        if (speed_state_ == Speed_State.Shift)
        {
            speed_ = shift_speed_;
        }
        if (speed_state_ == Speed_State.Base)
        {
            speed_ = base_speed_;
        } 
    }
    public bool Get_Shift_Position(out Vector2 shift_position_)
    {
        shift_position_ = transform.position;

        Vector2 player_size_ = Player.main_player_.GetComponent<Moveable>().Get_Size();
        Vector2 ray_origin_ = transform.position;

        RaycastHit2D hit_point_right = Physics2D.Raycast(ray_origin_, Vector2.right, player_size_.x / 2, 256, 0, 0);
        RaycastHit2D hit_point_up = Physics2D.Raycast(ray_origin_, Vector2.up, player_size_.y / 2, 256, 0, 0);
        RaycastHit2D hit_point_left;
        RaycastHit2D hit_point_down;

        if(hit_point_right.collider != null)
        {
            shift_position_.x = hit_point_right.point.x - player_size_.x / 2;
            hit_point_left = Physics2D.Raycast(ray_origin_, Vector2.left, player_size_.x, 256, 0, 0);
            if(hit_point_left.collider != null && hit_point_left.distance + hit_point_right.distance < player_size_.x)
            {
                shiftable_ = false;
            }
        }
        else
        {
            hit_point_left = Physics2D.Raycast(ray_origin_, Vector2.left, player_size_.x/2, 256, 0, 0);
            if (hit_point_left.collider != null)
            {
                shift_position_.x = hit_point_left.point.x + player_size_.x / 2;
            }
        }

        if (hit_point_up.collider != null)
        {
            shift_position_.y = hit_point_up.point.y - player_size_.y / 2;
            hit_point_down = Physics2D.Raycast(ray_origin_, Vector2.down, player_size_.y, 256, 0, 0);
            if (hit_point_down.collider != null && hit_point_down.distance + hit_point_up.distance < player_size_.y)
            {
                shiftable_ = false;
            }
        }
        else
        {
            hit_point_down = Physics2D.Raycast(ray_origin_, Vector2.down, player_size_.y/2, 256, 0, 0);
            if (hit_point_down.collider != null)
            {
                shift_position_.y = hit_point_down.point.y + player_size_.y / 2;
            }
        }
        
        return shiftable_;

    }
    public State Get_State()
    {
        return state_;
    }

}
