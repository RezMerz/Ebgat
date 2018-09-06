using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPhysic : Physic
{
    public Action<RaycastHit2D> BulletAction;
    public BulletShape shape;
    private RaycastHit2D hitObject;
    private float radius;

    void Start()
    {
        virtualPosition = transform.position;
        if (shape == BulletShape.Rectangle)
        {
            size = transform.localScale * GetComponent<BoxCollider2D>().size;
        }
        if (shape == BulletShape.Circle)
        {
            radius = transform.localScale.x * GetComponent<CircleCollider2D>().radius;
        }
    }
    private void FixedUpdate()
    {
        BulletAction += HitFunction;
        Calculate();
    }
    public void SetData(int layer)
    {
        layerMask = layer;
        Debug.Log(layerMask);
    }
    protected override void Calculate()
    {
        if (shape == BulletShape.Circle)
        {
            hitObject = Physics2D.CircleCast(virtualPosition, radius, distance.normalized, distance.magnitude, layerMask, 0, 0);
            if (hitObject.collider != null)
            {
                distance = distance * hitObject.fraction;
            }
        }
        if (shape == BulletShape.Rectangle)
        {
            hitObject = Physics2D.BoxCast(virtualPosition, size, 0, distance.normalized, distance.magnitude, layerMask, 0, 0);
            if (hitObject.collider != null)
            {
                distance = distance * hitObject.fraction;
            }
        }
        virtualPosition += distance;
        transform.position = virtualPosition;
        if (hitObject.collider != null)
        {
            BulletAction(hitObject);
        }
        distance = Vector2.zero;
        BulletAction = null;
    }
    private void HitFunction(RaycastHit2D hit2d)
    {
    }
}

public enum BulletShape { Circle, Rectangle }
