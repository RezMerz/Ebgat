using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Bullet : MonoBehaviour
{
    public float speed;

    private float range;
    private int ID;
    private float gravitySpeedBase;
    private float gravityAcceleration;
    private BulletPhysic physic;
    private float distance;
    private Vector2 distanceVector;
    private Vector2 direction;
    private bool shot;
    private Animator animator;

    private bool hitAnimation;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        physic = GetComponent<BulletPhysic>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (shot)
        {
            Move();
        }
    }

    public void Shoot(Vector2 direction, int layer, float gravityAcc, float range)
    {
        shot = true;
        physic.SetData(layer);
        this.direction = direction;
        this.range = range;
        gravityAcceleration = gravityAcc;
    }

    private void Move()
    {
        Vector2 force = direction.normalized * speed * Time.deltaTime;
        distanceVector += force;
        distance = distanceVector.magnitude;
        Vector2 gravityForce = Vector2.zero;
        if (distance > range)
        {
            gravityForce = Vector2.down * gravitySpeedBase * Time.deltaTime;
            gravitySpeedBase += gravityAcceleration * Time.deltaTime;
        }
        float angle = Vector2.SignedAngle(Vector2.right,(force + gravityForce).normalized);
        float angleX = 0;
        if(angle > 90 || angle < -90)
        {
            angleX = 180;
            angle = -angle;
        }
        transform.rotation = Quaternion.Euler(angleX,0,angle);
        physic.AddForce(force + gravityForce);
        physic.BulletAction += HitFunction;
    }

    private void HitFunction(RaycastHit2D hitObject)
    {

        if (hitObject.collider != null)
        {
            transform.localScale = Vector3.one;

            shot = false;
            Vector2 hitsSide = Toolkit.HitSide(hitObject);
            if(hitsSide == Vector2.right)
            {
                transform.rotation = Quaternion.Euler(0,0,-90);
            }
            else if (hitsSide == Vector2.left)
            {
                transform.rotation = Quaternion.Euler(0, 0, 90);
            }
            else if (hitsSide == Vector2.down)
            {
                transform.rotation = Quaternion.Euler(0, 0, 180);
            }
            else if(hitsSide == Vector2.up)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            hitAnimation = true;
            animator.SetTrigger("Hit");
        }
    }
    

    public void DestroyAnimation()
    {
        if (!hitAnimation)
        {
            transform.localScale = Vector3.one;
            shot = false;
            animator.SetTrigger("Hit");
        }
    }

    public void Destroy()
    {
       // Destroy(gameObject);
    }





}
