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
    protected BulletPhysic physic;
    private float distance;
    private Vector2 distanceVector;
    private Vector2 direction;
    protected bool shot;
    protected Animator animator;

    private float changeBehaviourRange;
    protected int layer;

    private bool hitAnimation;
    protected bool changed;
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

    public void Shoot(Vector2 direction, int layer, float gravityAcc, float range,float changeRange)
    {
        shot = true;
        physic.SetData(layer);
        this.layer = layer;
        this.direction = direction;
        this.range = range;
        changeBehaviourRange = changeRange;
        gravityAcceleration = gravityAcc;
    }

    public virtual void ChangeBehaviour()
    {
        changed = true;
    }

    private void Move()
    {
        Vector2 force = direction.normalized * speed * Time.deltaTime;
        distanceVector += force;
        distance = distanceVector.magnitude;
        if (!changed &&  distance >= changeBehaviourRange)
        {
            ChangeBehaviour();
        }

        Vector2 gravityForce = Vector2.zero;
        if (distance > range)
        {
            gravityForce = Vector2.down * gravitySpeedBase * Time.deltaTime;
            gravitySpeedBase += gravityAcceleration * Time.deltaTime;
        }
        float angle = Vector2.SignedAngle(Vector2.right, (force + gravityForce).normalized);
        float angleX = 0;
        if (angle > 90 || angle < -90)
        {
            angleX = 180;
            angle = -angle;
        }
        transform.rotation = Quaternion.Euler(angleX, 0, angle);
        physic.AddForce(force + gravityForce);
        physic.BulletAction += HitFunction;
    }

    private void HitFunction(RaycastHit2D hitObject)
    {

        if (hitObject.collider != null)
        {
            Vector2 hitsSide = Toolkit.HitSide(hitObject);
            HitWithSide(hitsSide);
            if (hitObject.collider.tag.Equals("Bullet"))
            {
                hitObject.collider.gameObject.GetComponent<Bullet>().HitWithSide(hitsSide * -1);
            }
        }
    }

    public virtual void HitWithSide(Vector2 hitSide)
    {
        shot = false;
        GetComponent<Collider2D>().enabled = false;
        transform.localScale = Vector3.one;
        if (hitSide == Vector2.right)
        {
            transform.rotation = Quaternion.Euler(0, 0, -90);
        }
        else if (hitSide == Vector2.left)
        {
            transform.rotation = Quaternion.Euler(0, 0, 90);
        }
        else if (hitSide == Vector2.down)
        {
            transform.rotation = Quaternion.Euler(0, 0, 180);
        }
        else if (hitSide == Vector2.up)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        hitAnimation = true;
        animator.SetTrigger("Hit");
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
