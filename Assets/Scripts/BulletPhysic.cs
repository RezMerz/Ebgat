using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysic : Physic {
    public BulletShape bulletShape;
    private float radius;

    // Use this for initialization
    void Start ()
    {
		if(bulletShape == BulletShape.Rectangle)
        {
            size = transform.localScale * GetComponent<BoxCollider2D>().size;
        }
        if (bulletShape == BulletShape.Circle)
        {
            radius = transform.localScale.x * GetComponent<CircleCollider2D>().radius;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
    protected override void Calculate()
    {
        base.Calculate();
    }

    protected override void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        throw new NotImplementedException();
    }
}

public enum BulletShape {Circle,Rectangle}
