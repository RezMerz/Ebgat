﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class Bullet : MonoBehaviour {
    public float damage;
    public float range;
    public float speed;
    public bool useGravity;
    public float gravitySpeedBase;
    public float gravityAcceleration;
    public Buff buff;

    private BulletPhysic physic;
    private float distance;
    private Vector2 distanceVector;
    private Vector2 direction;
    private bool shot;
    private bool hit;
    private RangedAttack rangedAttack;

    public int ID;

    void Start()
    {
        physic = GetComponent<BulletPhysic>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shot)
        {
            MoveServerSide();
        }
    }

    public void Shoot(Vector2 direction,PlayerControl pl)
    {
        physic.GetPlayerControl(pl);
        shot = true;
        this.direction = direction;
    }

    private void MoveServerSide()
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
            Vector2 gravityForce = Vector2.down * gravitySpeedBase * Time.deltaTime;
            gravitySpeedBase += gravityAcceleration * Time.deltaTime;
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
        if(hitObject.collider != null && hitObject.collider.tag.Equals("Player"))
        {
            var enemy = hitObject.collider.gameObject;
            string name = "";
            if(buff != null)
            {
                name = buff.name;
            }
            enemy.GetComponent<PlayerControl>().TakeAttack(damage,name);
        }
    }

    private void Destroy()
    {

    }

    

}
