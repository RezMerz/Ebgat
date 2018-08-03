using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    public float leftBorder;
    public float rightBorder;
    public float bottomBorder;
    public float topBorder;

    private Transform objectTransform;


	void Start ()
    {
        objectTransform = transform;	
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(objectTransform.position.x < leftBorder)
        {
            objectTransform.position = new Vector2(rightBorder, objectTransform.position.y);
        }
        if (objectTransform.position.x > rightBorder)
        {
            objectTransform.position = new Vector2(leftBorder, objectTransform.position.y);
        }
        if (objectTransform.position.y < bottomBorder)
        {
            objectTransform.position = new Vector2(objectTransform.position.x,topBorder);
        }
        if (objectTransform.position.y > topBorder)
        {
            objectTransform.position = new Vector2(objectTransform.position.x,bottomBorder);
        }
    }
}
