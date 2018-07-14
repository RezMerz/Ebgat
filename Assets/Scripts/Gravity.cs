using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
    public bool on_ground_;
    public float gravity_scale_;
    public float gravity_max_speed_;
    public float cayote_time_;
    bool active_ = true;
    bool hovering_;
    bool hit_;
    float distance_;
    float gravity_speed_;
    float base_sacle_;
    float base_max_speed_;
    float hovering_time_;
    float actual_speed_;
    float timer_;

    // Use this for initialization
    void Start()
    {
        actual_speed_ = gravity_scale_;
        base_sacle_ = gravity_scale_;
        gravity_speed_ = gravity_scale_;
        base_max_speed_ = gravity_max_speed_;
    }

    // Update is called once per frame
    void Update()
    {
        Speed_Check();
        Gravity_Call();
       // Hovering();
    }
    void Gravity_Call()
    {
        distance_ = gravity_speed_ * Time.deltaTime;
        if (active_)
        {
            hit_ = GetComponent<Moveable>().Move_Down(distance_);
            if (hit_)
            {
                timer_ = 0;
            }
            else
            {
                timer_ += Time.deltaTime;
            }
            if(timer_ >= cayote_time_)
            {
                on_ground_ = false;
            }
            else
            {
                on_ground_ = true;
            }
        }
        else
        {
            on_ground_ = false;
            timer_ += Time.deltaTime;
        }
    }
    void Speed_Check()
    {
        if (on_ground_)
        {
            gravity_speed_ = gravity_scale_;
            actual_speed_ = gravity_scale_;
        }
        else
        {
            actual_speed_ += gravity_scale_;
            if(gravity_speed_ < gravity_max_speed_)
            {
                gravity_speed_ += gravity_scale_;
            }
            if(gravity_speed_ > gravity_max_speed_)
            {
                gravity_speed_ -= 1.5f*(base_sacle_ - gravity_scale_);
            }
        }
    }
    void Hovering()
    {
        if (hovering_)
        {

            hovering_time_ -= Time.deltaTime;
            if (hovering_time_ <= 0)
            {
                hovering_ = false;
                active_ = true;
            }
            else
            {
                active_ = false;
            }
        }
    }
    public void Hover(float time)
    {
        hovering_time_ = time;
        hovering_ = true;
    }
    public void Activation()
    {
        active_ = true;
        gravity_speed_ = gravity_scale_;
    }
    public void Deactivition()
    {
        active_ = false;
    }
    public float Get_Distance()
    {
        return actual_speed_ * Time.deltaTime;
    }
    public void Speed_Reset()
    {
        gravity_speed_ = gravity_scale_;
    }
    public void Change_Properties(float scale,float max_speed)
    {
        if (scale == -1) scale = base_sacle_;
        if (max_speed == -1) max_speed = base_max_speed_;
        gravity_scale_ = scale;
        gravity_max_speed_ = max_speed; 

    }

}                   
