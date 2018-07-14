using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Moveable : MonoBehaviour {
    public float threshold_;

    float size_x;
    float size_y;

	void Start ()
    {
        Set_Size();
	}
    void Set_Size()
    {
        size_x = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        size_y = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
    }
    public Vector2 Get_Size()
    {
        return new Vector2(size_x, size_y);
    }
    public bool Move_Down(float distance)
    {
        bool hit = false ;
        float j = 0;
        Vector2 max_hit_point = new Vector2(0, -Mathf.Infinity);
        Vector2 ray_origin = transform.position;
        ray_origin.x -= (size_x / 2);
        for (int i = 0; i <= size_x; i++)
        {
            float k = 0;
            if (i == 0) k = threshold_;
            if (i == size_x) k = -threshold_;
            RaycastHit2D hit_point = Physics2D.Raycast(ray_origin + Vector2.right * (i+k), Vector2.down, distance + size_y / 2, 256, 0, 0);
            if (hit_point.collider != null && hit_point.point.y > max_hit_point.y)
            {
                hit = true;
                j = i+k;
                max_hit_point = hit_point.point;
            }
        }
        if (hit)
        {
            transform.position = max_hit_point + new Vector2(size_x / 2 - j, size_y / 2);
        }
        else
        {
            transform.position += Vector3.down * distance;
        }
        return hit;

    }
    public bool Move_Up(float distance,bool grab)
    {
        bool hit = false;
        float j = 0;
        Vector2 max_hit_point = new Vector2(0, Mathf.Infinity);
        Vector2 ray_origin = transform.position;
        ray_origin.x -= (size_x / 2);
        for (int i = 0; i <= size_x; i++)
        {
            float k = 0;
            if (i == 0) k = threshold_;
            if (i == size_x) k = -threshold_;
            RaycastHit2D hit_point = Physics2D.Raycast(ray_origin + Vector2.right * (i+k), Vector2.up, distance + size_y / 2, 256, 0, 0);
            if (hit_point.collider != null && hit_point.point.y < max_hit_point.y)
            {
                hit = true;
                j = i+k;
                max_hit_point = hit_point.point;
            }
        }
        if (hit)
        {
            transform.position = max_hit_point + new Vector2(size_x / 2 - j, -size_y / 2);
        }
        else if(!grab)
        {
            transform.position += Vector3.up * distance;
        }
        return hit;

    }
    public bool Move_Right(float distance,bool grab)
    {
        bool hit = false;
        float j = 0;
        Vector2 max_hit_point = new Vector2(Mathf.Infinity,0);
        Vector2 ray_origin = transform.position;
        ray_origin.y -= (size_y / 2);
        for (int i = 0; i <= size_y; i++)
        {
            float k = 0;
            if (i == 0) k = threshold_;
            if (i == size_y) k = -threshold_;
            RaycastHit2D hit_point = Physics2D.Raycast(ray_origin + Vector2.up * (i+k), Vector2.right, distance + size_x / 2, 256, 0, 0);
            if (hit_point.collider != null && hit_point.point.x < max_hit_point.x)
            {
                hit = true;
                j = i+k;
                max_hit_point = hit_point.point;
            }
        }
        if (hit)
        {
            transform.position = max_hit_point + new Vector2(-size_x/2, size_y / 2 - j);
        }
        else if(!grab)
        {
            transform.position += Vector3.right * distance;
        }
        return hit;

    }
    public bool Move_Left(float distance,bool grab)
    {
        bool hit = false;
        float j = 0;
        Vector2 max_hit_point = new Vector2(-Mathf.Infinity, 0);
        Vector2 ray_origin = transform.position;
        ray_origin.y -= (size_y / 2);
        for (int i = 0; i <= size_y; i++)
        {
            float k = 0;
            if (i == 0) k = threshold_;
            if (i == size_y) k = -threshold_;
            RaycastHit2D hit_point = Physics2D.Raycast(ray_origin + Vector2.up * (i+k), Vector2.left, distance + size_x / 2, 256, 0, 0);
            if (hit_point.collider != null && hit_point.point.x > max_hit_point.x)
            {
                hit = true;
                j = i+k;
                max_hit_point = hit_point.point;
            }
        }
        if (hit)
        {
            transform.position = max_hit_point + new Vector2(size_x / 2, size_y / 2 - j);
        }
        else if(!grab)
        {
            transform.position += Vector3.left * distance;
        }
        return hit;

    }
}
