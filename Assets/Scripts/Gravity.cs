using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
    public bool on_ground_;

    // switch to turn on/off gravity
    public bool active = true;

    // Base Variable to set to defaults whenever needed
    public  float baseSpeed;
    public float baseMaxSpeed;
    public float baseAcceleration;
    public float baseCayoteTime;

    // Variables of that bases
    private float acceleration;
    private float maxSpeed;
    private float speed;
    private float cayoteTime;
    

    private List<RaycastHit2D> hitObjects;
    bool hovering_;
    bool hit_;
    float distance_;

    float hovering_time_;
    float actual_speed_;
    float timer_;

    // Use this for initialization
    void Start()
    {
        actual_speed_ = acceleration;

        acceleration = baseAcceleration;
        speed = baseSpeed;
        maxSpeed = baseMaxSpeed;


    }

    // Update is called once per frame
    void Update()
    {
        HeroGravity();
       // Speed_Check();
       // Gravity_Call();
       // Hovering();
    }

    private void HeroGravity()
    {

    }

    public void SetToDefaults()
    {
        acceleration = baseAcceleration;
        speed = baseSpeed;
        maxSpeed = baseMaxSpeed;
    }
    void Gravity_Call()
    {
        distance_ = speed * Time.deltaTime;
        if (active)
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
            if(timer_ >= cayoteTime)
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
            speed = acceleration;
            actual_speed_ = acceleration;
        }
        else
        {
            actual_speed_ += acceleration;
            if(speed < maxSpeed)
            {
                speed += acceleration;
            }
            if(speed > maxSpeed)
            {
                speed -= 1.5f*(baseAcceleration - acceleration);
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
                active = true;
            }
            else
            {
                active = false;
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
        active = true;
        speed = acceleration;
    }
    public void Deactivition()
    {
        active = false;
    }
    public float Get_Distance()
    {
        return actual_speed_ * Time.deltaTime;
    }
    public void Speed_Reset()
    {
        speed = acceleration;
    }
    public void Change_Properties(float scale,float max_speed)
    {
        if (scale == -1) scale = baseAcceleration;
        if (max_speed == -1) max_speed = baseMaxSpeed;
        acceleration = scale;
        maxSpeed = max_speed; 

    }

}                   
