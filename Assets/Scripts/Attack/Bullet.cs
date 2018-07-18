using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    private float damage;
    public float range;
    public float width { get; set; }
    public float gravitySpeedBase;
    public float gravityAcceleration;
    public float speed;

    private Vector2 direction;

    private Vector2 vDirection;
    private Vector2 hDirection;

    private float gravitySpeed;
    private float distance;

    private Vector2 vMove;
    private Vector2 size;
    private bool shot;
    private bool hit;
    private float vSpeed;
    private List<RaycastHit2D> hitObjects;
    void Start()
    {
        hit = false;
        size = GetComponent<BoxCollider2D>().size * transform.localScale;
        
    }
    public void Shoot(Vector2 targetDirection, Vector2 origin,float bulletDamage)
    {
        damage = bulletDamage;
        direction = (targetDirection - origin).normalized;
        // Move Right or Left
        hDirection = (Vector2.right * direction).normalized;
        transform.position = origin;
        shot = true; 
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();

	}
    
    private void Move()
    {
        if (shot)
        {
            
            hit = Toolkit.CheckMoveFloat(transform.position, size, hDirection, Time.deltaTime * speed * Mathf.Abs(direction.x), 256, out hitObjects);
            if (hit)
            {
                transform.position += (Vector3)hDirection * hitObjects[0].distance;
                Hit();
                return;
            }
            else
            {
                transform.position += (Vector3)hDirection * Time.deltaTime * speed * Mathf.Abs(direction.x);
            }

            // Calculations to see the bullet should go down or up
            gravitySpeed += gravityAcceleration;
            vMove = (Vector2.up * direction) * speed + (Vector2.down * gravitySpeed);
            vDirection = vMove.normalized;
            vSpeed = Mathf.Abs((vMove).y);
            hit = Toolkit.CheckMoveFloat(transform.position, size, vDirection, Time.deltaTime * vSpeed, 256, out hitObjects);
            if (hit)
            {
                transform.position += (Vector3)vDirection * hitObjects[0].distance;
                Hit();
                return;
            }
            else
            {
                transform.position += (Vector3)vDirection * Time.deltaTime * vSpeed;
            }

        }
    }

    private void Hit()
    {
        shot = false;
        if (hitObjects[0].collider.tag == "Player")
        {
            hitObjects[0].collider.gameObject.GetComponent<PlayerControl>().TakeAttack(damage, null);
        }
        Destroy(gameObject);
    }
    
}
