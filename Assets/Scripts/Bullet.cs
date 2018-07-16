using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float damage;
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

    private Vector2 size;
    private bool shot;
    private bool hitV;
    private bool hitH;
    private float vSpeed;
    private List<RaycastHit2D> hitObjects;
    void Start()
    {
        hitV = false;
        size = GetComponent<BoxCollider2D>().size * transform.localScale;
        
    }
    public void Shoot(Vector2 targetDirection, Vector2 origin)
    {
        direction = (targetDirection - origin).normalized;
        hDirection = (Vector2.right * direction).normalized;

        vSpeed = speed * Mathf.Abs(direction.y);



        transform.position = origin;
        shot = true;
       
        
        
    }
	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
    {
        Move();
		// Check Move
        // Move
        // if hit damage hero

	}
    
    private void Move()
    {
        if (shot)
        {
            gravitySpeed += gravityAcceleration;
            vDirection = ((Vector2.up * direction) * speed + Vector2.down * gravitySpeed).normalized;
            vSpeed = Mathf.Abs(((Vector2.up * direction) *speed  + Vector2.down * gravitySpeed).y);
            print(vDirection);
            hitH = Toolkit.CheckMoveFloat(transform.position, size, hDirection, Time.deltaTime * speed * Mathf.Abs(direction.x), 256, out hitObjects);
            if (hitH)
            {
                transform.position += (Vector3)hDirection * hitObjects[0].distance;
                shot = false;
                return;
            }
            else
            {
                transform.position += (Vector3)direction * Time.deltaTime * speed * Mathf.Abs(direction.x);
            }
            hitV = Toolkit.CheckMoveFloat(transform.position, size, vDirection, Time.deltaTime * vSpeed, 256, out hitObjects);
            if (hitV)
            {
                transform.position += (Vector3)vDirection * hitObjects[0].distance;
                shot = false;
                return;
            }
            else
            {
                transform.position += (Vector3)vDirection * Time.deltaTime * vSpeed;
            }

        }
    }
    
}
