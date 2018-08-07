using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Physic : MonoBehaviour {
    public float wight;

    protected int layerMask;
    protected int gravityLayerMask;

    protected Vector2 size;
    protected Vector2 distance;
    protected Vector2 virtualPosition;

    protected List<Vector2> forces = new List<Vector2>();
    protected List<Vector2> destenitions = new List<Vector2>();
    protected List<RaycastHit2D> hitPoints;

    protected PlayerControl playerControl;

	// Use this for initialization
	private void Start ()
    {
        size = transform.localScale * GetComponent<BoxCollider2D>().size;
	}

    private void Update()
    {
        if (!playerControl.IsServer())
        {
            Predict();
        }
    }

    private void LateUpdate()
    {
        if (playerControl.IsServer())
        {
            Calculate();
        }
    } 
    private void Calculate()
    {
        bool hit;
        distance = Vector2.zero;
        while (forces.Count > 0)
        {
            var currentforce = forces[0];
            distance += currentforce;
            forces.Remove(currentforce);
        }
        if (distance.x > 0)
        {
            hit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.right, distance.x, layerMask, out hitPoints);
            if (hit)
            {
                HitFunction(hitPoints,Vector2.right);
            }
        }
        else if (distance.x < 0)
        {
            hit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.left, -distance.x, layerMask, out hitPoints);
            if (hit)
            {
                HitFunction(hitPoints,Vector2.left);
            }
        }
        if (distance.y > 0)
        {
            hit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.up, distance.y, layerMask, out hitPoints);
            if (hit)
            {
                HitFunction(hitPoints,Vector2.up);
            }
        }
        else if (distance.y < 0)
        {
            hit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.down, -distance.y, layerMask, out hitPoints);
            if (hit)
            {
                HitFunction(hitPoints,Vector2.down);
            }
        }
        virtualPosition += distance;
        destenitions.Add(virtualPosition);

    }
    private void Predict()
    {

    }
    public void AddForce(Vector2 force)
    {
        forces.Add(force);
    }

    abstract protected void HitFunction(List<RaycastHit2D> hits,Vector2 direction);




}

