using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour {
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
    private bool hit;
    private List<RaycastHit2D> hitObjects;
    void Start()
    {
        hit = false;
        size = transform.localScale;
        width = GetComponent<BoxCollider2D>().size.x * transform.localScale.x;
    }
    public void Shoot(Vector2 targetDirection, Vector2 origin)
    {
        direction = (targetDirection - origin).normalized;
        transform.position = origin;
        shot = true;
       
        
        
    }
	// Use this for initialization
	
	// Update is called once per frame
	void Update ()
    {
        if (shot)
        {
            hit = Toolkit.CheckMoveFloat(transform.position, size, direction, Time.deltaTime * speed, 256, out hitObjects);
            if (hit)
            {
                transform.position += (Vector3)direction * Time.deltaTime * speed;
                shot = false;
            }
            else
            {
                transform.position += (Vector3)direction * Time.deltaTime * speed;
            }
           
        }
		// Check Move
        // Move
        // if hit damage hero

	}
    
    private void Move()
    {

    }
    
}
