﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolkit : MonoBehaviour {
    ///  Vector 2 Transpose
    public static  Vector2 Transpose2(Vector2 vec){
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

    public static bool CheckMoveFloat(Vector2 originPosition, Vector2 vecSize, Vector2 direction, float distance, int layerNumber, out List<RaycastHit2D> hitPoints)
    {
        float threshold = 0.01f;
        bool hit = false;
        Vector2 ceilingSize = new Vector2(vecSize.x / Mathf.Ceil(vecSize.x), vecSize.y / Mathf.Ceil(vecSize.y));

        List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
        Vector2 rayOrigin = originPosition + vecSize * (direction.x + direction.y) / 2;
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
                hit = true;
                hitObjects.Add(hitPoint);
            }
        }
        hitObjects.Sort(new HitDistanceCompare());
        hitPoints = hitObjects;
        return hit;
    }
    public static float FloatCut(float f)
    {
        return (int)(f * 100f) / 100f;
    }
    
}
