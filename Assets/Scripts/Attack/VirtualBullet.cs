using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBullet : MonoBehaviour
{

    public Buff buff;
    public float speed;
    public bool useGravity;
    public float range;

    private int ID;
    private float damage;
    private float gravitySpeedBase;
    private float gravityAcceleration;
    private BulletPhysic physic;
    private PlayerControl playerControl;
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
            MoveServerSide();
        }
    }

    public void Shoot(float damage, Vector2 direction,int layer, float gravityAcc,PlayerControl pl,int id)
    {
        Debug.Log("shot");
        shot = true;
        physic.SetData(layer);
        this.direction = direction;
        this.damage = damage;
        this.range = range;
        gravityAcceleration = gravityAcc;
        playerControl = pl;
        ID = id;
    }

    private void MoveServerSide()
    {
        if (distance < range)
        {
            Debug.Log("move");
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
        Debug.Log(hitObject.collider);
        if (hitObject.collider.tag.Equals("Player"))
        {
            var enemy = hitObject.collider.gameObject;
            string name = "";
            if (buff != null)
            {
                name = buff.name;
            }
            enemy.GetComponent<PlayerControl>().TakeAttack(damage, name);
            Destroy();
        }
        else
        {
            Destroy();
        }
    }

    private void Destroy()
    {

        /// send destroyed massage
        Destroy(gameObject);
        playerControl.worldState.BulletHit(playerControl.playerId, ID);
        
    }
}
