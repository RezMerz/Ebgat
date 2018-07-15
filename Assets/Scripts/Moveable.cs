using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Moveable : MonoBehaviour {
    public float threshold;

    float sizeX;
    float sizeY;

	void Start ()
    {
        Set_Size();
	}
    void Set_Size()
    {
        sizeX = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        sizeY = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
    }
    public Vector2 Get_Size()
    {
        return new Vector2(sizeX, sizeY);
    }
    
    // ray cast for move in square objects
    public bool CheckMove(Vector2 direction, float distance, int layerNumber,out List<RaycastHit2D> hitPoints)
    {
        bool hit = false;
        List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
        Vector2 rayOrigin = transform.position;
        rayOrigin -= Toolkit.Transpose2(direction) * Get_Size() / 2;
        Vector2 multiplier = Toolkit.Transpose2(direction);
        float loopSize = Mathf.Abs(direction.x) * sizeY + Mathf.Abs(direction.y) * sizeX;
        float size = Mathf.Abs(direction.x) * sizeX + Mathf.Abs(direction.y) * sizeY;
        for (int i = 0; i <= loopSize; i++)
        {
            float k = 0;
            // first point threshold
            if (i == 0)
                k = threshold;
            // last point threshold
            if (i == loopSize)
                k = -threshold;
            RaycastHit2D hitPoint = Physics2D.Raycast(rayOrigin + multiplier * (i + k), direction, distance + size / 2, layerNumber, 0, 0);
            if (hitPoint.collider != null)
            {
                hit = true;
                hitObjects.Add(hitPoint);
            }
        }
        hitObjects.Sort(new HitDistanceCompare());
        hitPoints = hitObjects;
        return hit;
    }

    public void Move(Vector2 direction, float distance, List<RaycastHit2D> hitObjects)
    {
        // hit nothing , move at distance
        if(hitObjects.Count == 0)
        {
            transform.position += distance * (Vector3)direction;
        }
        else
        {
            float size = Mathf.Abs(direction.x) * sizeX + Mathf.Abs(direction.y) * sizeY;
            transform.position += (Vector3)direction * (hitObjects[0].distance - size / 2);
        }
    }
    public bool Move_Down(float distance)
    {
        bool hit = false ;
        float j = 0;
        Vector2 max_hit_point = new Vector2(0, -Mathf.Infinity);
        Vector2 ray_origin = transform.position;
        ray_origin.x -= (sizeX / 2);
        for (int i = 0; i <= sizeX; i++)
        {
            float k = 0;
            if (i == 0) k = threshold;
            if (i == sizeX) k = -threshold;
            RaycastHit2D hit_point = Physics2D.Raycast(ray_origin + Vector2.right * (i+k), Vector2.down, distance + sizeY / 2, 256, 0, 0);
            if (hit_point.collider != null && hit_point.point.y > max_hit_point.y)
            {
                hit = true;
                j = i+k;
                max_hit_point = hit_point.point;
            }
        }
        if (hit)
        {
            transform.position = max_hit_point + new Vector2(sizeX / 2 - j, sizeY / 2);
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
        ray_origin.x -= (sizeX / 2);
        for (int i = 0; i <= sizeX; i++)
        {
            float k = 0;
            if (i == 0) k = threshold;
            if (i == sizeX) k = -threshold;
            RaycastHit2D hit_point = Physics2D.Raycast(ray_origin + Vector2.right * (i+k), Vector2.up, distance + sizeY / 2, 256, 0, 0);
            if (hit_point.collider != null && hit_point.point.y < max_hit_point.y)
            {
                hit = true;
                j = i+k;
                max_hit_point = hit_point.point;
            }
        }
        if (hit)
        {
            transform.position = max_hit_point + new Vector2(sizeX / 2 - j, -sizeY / 2);
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
        ray_origin.y -= (sizeY / 2);
        for (int i = 0; i <= sizeY; i++)
        {
            float k = 0;
            if (i == 0) k = threshold;
            if (i == sizeY) k = -threshold;
            RaycastHit2D hit_point = Physics2D.Raycast(ray_origin + Vector2.up * (i+k), Vector2.right, distance + sizeX / 2, 256, 0, 0);
            if (hit_point.collider != null && hit_point.point.x < max_hit_point.x)
            {
                hit = true;
                j = i+k;
                max_hit_point = hit_point.point;
            }
        }
        if (hit)
        {
            transform.position = max_hit_point + new Vector2(-sizeX/2, sizeY / 2 - j);
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
        ray_origin.y -= (sizeY / 2);
        for (int i = 0; i <= sizeY; i++)
        {
            float k = 0;
            if (i == 0) k = threshold;
            if (i == sizeY) k = -threshold;
            RaycastHit2D hit_point = Physics2D.Raycast(ray_origin + Vector2.up * (i+k), Vector2.left, distance + sizeX / 2, 256, 0, 0);
            if (hit_point.collider != null && hit_point.point.x > max_hit_point.x)
            {
                hit = true;
                j = i+k;
                max_hit_point = hit_point.point;
            }
        }
        if (hit)
        {
            transform.position = max_hit_point + new Vector2(sizeX / 2, sizeY / 2 - j);
        }
        else if(!grab)
        {
            transform.position += Vector3.left * distance;
        }
        return hit;

    }
}


public class HitDistanceCompare : IComparer<RaycastHit2D>
{
    public int Compare(RaycastHit2D x, RaycastHit2D y)
    {
        if (x.distance == y.distance)
            return 0;
        if (x.distance < y.distance)
            return -1;
        else return 1;
    }
}
