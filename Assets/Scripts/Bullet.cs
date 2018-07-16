using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public float damage;
    public float range;
    public float width;
    public float gravitySpeedBase;
    public float gravityAcceleration;
    public float Speed;

    public Vector2 direction { get; set; }

    private Vector2 vDirection;
    private Vector2 hDirection;

    private float gravitySpeed;
    private float distance;
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    
    private void Move()
    {

    }
    
}
