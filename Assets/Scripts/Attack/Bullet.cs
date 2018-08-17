using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Bullet : MonoBehaviour
{
    public bool useGravity;
    public float speed;
    public float range;

    private int ID;
    private float gravitySpeedBase;
    private float gravityAcceleration;
    private BulletPhysic physic;
    private float distance;
    private Vector2 distanceVector;
    private Vector2 direction;
    private bool shot;


    private void Awake()
    {
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

    public void Shoot(Vector2 direction, int layer, float gravityAcc)
    {
        shot = true;
        physic.SetData(layer);
        this.direction = direction;
        gravityAcceleration = gravityAcc;
    }

    private void Move()
    {
        if (distance < range)
        {
            Vector2 force = direction.normalized * speed * Time.deltaTime;
            distanceVector += force;
            distance = distanceVector.magnitude;
            if (distance > range)
            {
                force = direction.normalized * (distance - range);
                distance = range;
            }
            Vector2 gravityForce = Vector2.zero;
            if (useGravity)
            {
                gravityForce = Vector2.down * gravitySpeedBase * Time.deltaTime;
                gravitySpeedBase += gravityAcceleration * Time.deltaTime;
            }
            physic.AddForce(force + gravityForce);
            physic.BulletAction += HitFunction;
        }
        else
        {
            Destroy();
        }
    }

    private void HitFunction(RaycastHit2D hitObject)
    {
        if (hitObject.collider != null)
        {
            Destroy();
        }
    }

    private void Destroy()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }



}
