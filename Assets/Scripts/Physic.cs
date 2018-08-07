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
    protected List<RaycastHit2D> verticalPoints;
    protected List<RaycastHit2D> horizontalPoints;

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
        bool vHit = false ;
        bool hHit = false ;
        List<RaycastHit2D> verticalPoints = null;
        List<RaycastHit2D> horizontalPoints =null ;
        distance = Vector2.zero;
        while (forces.Count > 0)
        {
            var currentforce = forces[0];
            distance += currentforce;
            forces.Remove(currentforce);
        }
        if (distance.x > 0)
        {
            hHit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.right, distance.x, layerMask, out horizontalPoints);
        }
        else if (distance.x < 0)
        {
            hHit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.left, -distance.x, layerMask, out horizontalPoints);
        }
        if (distance.y > 0)
        {
            vHit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.up, distance.y, layerMask, out verticalPoints);
        }
        else if (distance.y < 0)
        {
            vHit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.down, -distance.y, layerMask, out verticalPoints);
        }
        if(hHit || vHit)
        {
            HitFunction(verticalPoints, horizontalPoints, distance);
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

    abstract protected void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction);




}

