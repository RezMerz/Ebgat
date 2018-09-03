using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class Toolkit : MonoBehaviour
{
    ///  Vector 2 Transpose
    public static Vector2 Transpose2(Vector2 vec)
    {
        return new Vector2(vec.y, vec.x);
    }

    public static bool CheckMove(Vector2 originPosition, Vector2 vecSize, Vector2 direction, float distance, int layerNumber, out List<RaycastHit2D> hitPoints)
    {
        float threshold = 0.01f;
        bool hit = false;
        List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
        Vector2 rayOrigin = originPosition + vecSize * (direction.x + direction.y) / 2;
        Vector2 multiplier = Transpose2(direction);
        float loopSize = Mathf.Abs(direction.x) * vecSize.y + Mathf.Abs(direction.y) * vecSize.x;
        for (int i = 0; i <= loopSize; i++)
        {
            float k = 0;
            // first point threshold
            if (i == 0)
                k = threshold;
            // last point threshold
            if (i == loopSize)
                k = -threshold;
            RaycastHit2D hitPoint = Physics2D.Raycast(rayOrigin - multiplier * (i + k), direction, distance, layerNumber, 0, 0);
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

    public static bool CheckMoveFloat(Vector2 originPosition, Vector2 vecSize, Vector2 offset, Vector2 direction, float distance, int layerNumber, out List<RaycastHit2D> hitPoints)
    {
        float threshold = 0.01f;
        bool hit = false;
        Vector2 ceilingSize = new Vector2(vecSize.x / Mathf.Ceil(vecSize.x), vecSize.y / Mathf.Ceil(vecSize.y));

        List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
        Vector2 rayOrigin = originPosition + vecSize * (direction.x + direction.y) / 2 + offset;
        Vector2 multiplier = Transpose2(direction) * ceilingSize;
        float loopSize = Mathf.Abs(direction.x) * Mathf.Ceil(vecSize.y) + Mathf.Abs(direction.y) * Mathf.Ceil(vecSize.x);
        for (int i = 0; i <= loopSize; i++)
        {
            float k = 0;
            // first point threshold
            if (i == 0)
                k = threshold;
            // last point threshold
            if (i == loopSize)
                k = -threshold;
            RaycastHit2D hitPoint = Physics2D.Raycast(rayOrigin - multiplier * (i + k), direction, distance, layerNumber, 0, 0);
            if (hitPoint.collider != null)
            {
                if (!hitPoint.collider.tag.Equals("Sword") && !hitPoint.collider.tag.Equals("Bullet"))
                {
                    if (!hitPoint.collider.tag.Equals("Bridge") || HitUpCheck(hitPoint))
                    {
                        hit = true;
                        hitObjects.Add(hitPoint);
                    }
                }
            }
        }
        hitObjects.Sort(new HitDistanceCompare());
        hitPoints = hitObjects;
        return hit;
    }
    public static bool OnWallCheck(Vector2 originPosition, Vector2 vecSize, Vector2 offset, Vector2 direction, float distance, int layerNumber)
    {
        float threshold = 0.4f;
        //bool onWall = false;
        Vector2 rayOriginFirst = (originPosition + offset) + (vecSize * direction / 2) + Vector2.up * (vecSize.y / 2 - threshold);
        Vector2 rayOriginSeconed = (originPosition + offset) + (vecSize * direction / 2) + Vector2.down * (vecSize.y / 2 + threshold);
        bool hitPointFirst = Physics2D.Raycast(rayOriginFirst, direction, distance, layerNumber, 0, 0);
        bool hitPointSecond = Physics2D.Raycast(rayOriginSeconed, direction, distance, layerNumber, 0, 0);
        return (hitPointFirst && hitPointSecond);
    }
    public static float FloatCut(float f)
    {
        return (int)(f * 100f) / 100f;
    }

    public static Vector2 HitSide(RaycastHit2D hit)
    {
        Vector2 side = Vector2.zero;
        Vector2 direction = hit.point - (Vector2)hit.transform.position;
        float angle = Vector2.SignedAngle(direction, Vector2.up);
        if (angle >= -45 && angle <= 45)
        {
            side = Vector2.up;
        }
        else if (angle > 45 && angle <= 135)
        {
            side = Vector2.right;
        }
        else if ((angle > 135 && angle <= 180) || (angle >= -180 && angle <= -135))
        {
            side = Vector2.down;
        }
        else if (angle > -135 && angle < -45)
        {
            side = Vector2.left;
        }
        return side;
    }

    public static bool HitUpCheck(RaycastHit2D hit)
    {
        Vector2 size = hit.collider.gameObject.GetComponent<BoxCollider2D>().size * hit.transform.localScale;
        if ((hit.point - ((Vector2)hit.transform.position + (size * 0.5f))) * Vector2.up == Vector2.zero)
        {
            return true;
        }
        return false;

    }

    public static string VectorSerialize(Vector2 vector)
    {
        return vector.x + "," + vector.y;
    }

    public static Vector2 DeserializeVector(string s)
    {
        string[] parts = s.Split(',');
        return new Vector2(float.Parse(parts[0], CultureInfo.InvariantCulture.NumberFormat), float.Parse(parts[1], CultureInfo.InvariantCulture.NumberFormat));
    }

    public static bool IsVisible(Vector2 origin,Vector2 destenition,int layerMask)
    {
        Vector2 directiohn = destenition - origin;
        RaycastHit2D hit = Physics2D.Raycast(origin, directiohn.normalized, directiohn.magnitude, layerMask, 0, 0);
        if (hit.collider.tag.Equals("VirtualPlayer"))
        {
            return true;
        }
        return false;
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
