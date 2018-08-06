using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Physic : MonoBehaviour {
    public float wight;
    private Vector2 size;
    private int layerMask;
    private int gravityLayerMask;

    private Vector2 distance;
    private List<Vector2> forces = new List<Vector2>();
    private List<Vector2> destenitions = new List<Vector2>();
    private PlayerControl playerControl;
    private Vector2 virtualPosition;

    private List<RaycastHit2D> hitPoints;

	// Use this for initialization
	private void Start ()
    {
        size = transform.localScale * GetComponent<BoxCollider2D>().size;
        playerControl = GetComponent<PlayerControl>();
    	    	
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
                distance.x = hitPoints[0].distance;
                HitFunction(hitPoints,Vector2.right);
            }
        }
        else if (distance.x < 0)
        {
            hit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.left, -distance.x, layerMask, out hitPoints);
            if (hit)
            {
                distance.x = -hitPoints[0].distance;
                HitFunction(hitPoints,Vector2.left);
            }
        }
        if (distance.y > 0)
        {
            hit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.up, distance.y, layerMask, out hitPoints);
            if (hit)
            {
                distance.x = -hitPoints[0].distance;
                HitFunction(hitPoints,Vector2.up);
            }
        }
        else if (distance.y < 0)
        {
            hit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.left, -distance.x, layerMask, out hitPoints);
            if (hit)
            {
                distance.x = -hitPoints[0].distance;
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

    abstract public void HitFunction(List<RaycastHit2D> hits,Vector2 direction);




}
