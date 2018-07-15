using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour {
    private CharacterAttributes charStats;
    public bool onGround;

    // switch to turn on/off gravity
    public bool active = true;

    // Base Variable to set to defaults whenever needed
    public float baseSpeed;
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
    float distance;

    float hovering_time_;
    float actual_speed_;
    float timer_;
   
    // Use this for initialization
    void Start()
    {
        hitObjects = new List<RaycastHit2D>();
        charStats = GetComponent<CharacterAttributes>();
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
        // not int jumping state
        if (charStats.FeetState != EFeetState.Jumping)
        {
            bool hit = Toolkit.CheckMove(transform.position, charStats.size, Vector2.down, Time.deltaTime * charStats.gravitySpeed, 256,out hitObjects);

            if (charStats.FeetState == EFeetState.Onground)
            {
                // Go to Falling
                if(!hit)
                {
                    charStats.FeetState = EFeetState.Falling;
                }

            }
            else if (charStats.FeetState == EFeetState.Falling)
            {
                // Go to on Ground state
                if(hit)
                {
                    transform.position -= new Vector3(0,(hitObjects[0].distance - charStats.size.y / 2));
                    charStats.FeetState = EFeetState.Onground;
                    /// Reset Gravity Stats
                }
                // Still Faliing
                else
                {
                    transform.position -= new Vector3(0, Time.deltaTime * charStats.gravitySpeed);
                }
            }
        }
    }
    void Gravity_Call()
    {
        distance = speed * Time.deltaTime;
        if (active)
        {
           // hit_ = GetComponent<Moveable>().Move(distance);
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
                onGround = false;
            }
            else
            {
                onGround = true;
            }
        }
        else
        {
            onGround = false;
            timer_ += Time.deltaTime;
        }
    }
    void Speed_Check()
    {
        if (onGround)
        {
            //set speed of gravity to zero if chracter is on ground
            speed = 0;
        }
        else
        {   
            if(speed < maxSpeed)
            {
                //increase speed of gravity over time
                speed += acceleration;
            }

            if(speed > maxSpeed)
            {
                //decrease speed of gravity if speed passes over speed limit
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
        speed = 0;
    }
    public void ChangeProperties(float acceleration,float maxSpeed)
    {
        this.acceleration = acceleration;
        this.maxSpeed = maxSpeed; 
    }

}                   
