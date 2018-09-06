using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualBullet : MonoBehaviour
{

    public Buff buff;
    public float speed;
    public float force;

    private float range;
    private int ID;
    private float damage;
    private float gravitySpeedBase;
    private float gravityAcceleration;
    protected BulletPhysic physic;
    private PlayerControl playerControl;
    private float distance;
    private Vector2 distanceVector;
    private Vector2 direction;
    private bool shot;
    private float changeBehaviourRange;
    protected bool changed;

    private Vector2 lastForce;
    protected int layer;

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

    public void Shoot(float damage, Vector2 direction, int layer, float gravityAcc, PlayerControl pl, int id,float range,float changeRange)
    {
        shot = true;
        this.layer = layer;
        physic.SetData(layer);
        this.direction = direction;
        this.damage = damage;
        this.range = range;
        changeBehaviourRange = changeRange;
        gravityAcceleration = gravityAcc;
        playerControl = pl;
        ID = id;
    }

    public virtual void ChangeBehaviour()
    {
        changed = true;
    }

    private void MoveServerSide()
    {
        Vector2 force = direction.normalized * speed * Time.deltaTime;
        distanceVector += force;
        distance = distanceVector.magnitude;
        if( !changed && distance >= changeBehaviourRange)
        {
            ChangeBehaviour();
        }

        Vector2 gravityForce = Vector2.zero;
        if (distance >= range)
        {
            gravityForce = Vector2.down * gravitySpeedBase * Time.deltaTime;
            gravitySpeedBase += gravityAcceleration * Time.deltaTime;
        }

        lastForce = force + gravityForce;
        physic.AddForce(force + gravityForce);
        physic.BulletAction += HitFunction;
    }

    private void HitFunction(RaycastHit2D hitObject)
    {
        
        if (hitObject.collider.tag.Equals("VirtualPlayer"))
        {
            var enemy = hitObject.collider.gameObject;
            string name = "";
            if (buff != null)
            {
                name = buff.name;
            }
            enemy.GetComponent<CharacterPhysic>().AddReductiveForce(lastForce.normalized, force, 0.1f, 0);
            Debug.Log(enemy);
            enemy.GetComponent<PlayerControl>().TakeAttack(damage, name);
            Destroy();
        }
        else if(hitObject.collider.tag.Equals("VirtualBullet"))
        {
            hitObject.collider.gameObject.GetComponent<VirtualBullet>().Destroy();
            Destroy();
        }
        else
        {
            Destroy();
        }
    }

    public void Destroy()
    {

        /// send destroyed massage
        playerControl.worldState.BulletHit(playerControl.playerId, ID);
        Destroy(gameObject);

    }
}
