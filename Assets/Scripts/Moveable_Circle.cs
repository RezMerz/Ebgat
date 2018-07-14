using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable_Circle : MonoBehaviour {
    public float radius_;
    public float threshold_;

	// Use this for initialization
	void Start ()
    {
        Get_Radius();
	}
	
	// Update is called once per frame
	void Update ()
    {

	}
    void Get_Radius()
    {
        radius_ = (transform.localScale.x * GetComponent<CircleCollider2D>().radius)-threshold_;
    }
    public bool Move(float distance,Vector3 direction)
    {
        bool hit = false;
        Vector2 ray_origin = transform.position;
        RaycastHit2D hit_point = Physics2D.CircleCast(ray_origin, radius_, direction,distance,256,0,0);
        if(hit_point.collider != null)
        {
            hit = true;
            //float angle_= Vector2.Angle(direction,hit_point.point-ray_origin);
            transform.position += (hit_point.distance) * direction;
            
        }
        else
        {
            transform.position += direction * distance;
        }
        return hit;
    }
}
