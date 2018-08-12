using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysic : Physic {
    public Action<RaycastHit2D> BulletAction;

    public BulletShape shape;
    private float radius;
    private RaycastHit2D hitObject;

    void Start ()
    {
		if(shape == BulletShape.Rectangle)
        {
            size = transform.localScale * GetComponent<BoxCollider2D>().size;
        }
        if (shape == BulletShape.Circle)
        {
            radius = transform.localScale.x * GetComponent<CircleCollider2D>().radius;
        }
	}
	void Update ()
    {
        BulletAction += HitFunction;
	}
    protected override void Calculate()
    {
        if(shape == BulletShape.Circle)
        {
            hitObject = Physics2D.CircleCast(virtualPosition, radius, distance.normalized, distance.magnitude, layerMask, 0, 0);
            if(hitObject.collider != null)
            {
                virtualPosition += distance.normalized * hitObject.fraction;
            }
            BulletAction(hitObject);
        }
        if (shape == BulletShape.Rectangle)
        {
            hitObject = Physics2D.BoxCast(virtualPosition, size, 0, distance.normalized, distance.magnitude, layerMask, 0, 0);
            if (hitObject.collider != null)
            {
                virtualPosition += distance.normalized * hitObject.fraction;
            }
            BulletAction(hitObject);
        }
        distance = Vector2.zero;
        BulletAction = null;
    }
    private void HitFunction(RaycastHit2D hit2d)
    {
        throw new NotImplementedException();
    }
}

public enum BulletShape {Circle,Rectangle}
