using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

abstract public class Physic : MonoBehaviour
{
    public float weight;
    protected int layerMask;

    protected Vector2 size;
    protected Vector2 distance;
    protected List<PersitentForce> persitentForces = new List<PersitentForce>();

    public Vector2 virtualPosition { get; set;}

    public void AddForce(Vector2 force)
    {
        distance += force;
    }
    public void AddPersistentForce(Vector2 force,float distance)
    {
        persitentForces.Add(new PersitentForce(force, distance));
    }

    protected void CheckPersitentForces()
    {
        for (int i = 0;i < persitentForces.Count; i++) 
        {
            var pForce = persitentForces[i];
            pForce.vectorSum += pForce.force * Time.deltaTime;
            if(pForce.vectorSum.magnitude > pForce.distance)
            {
                AddForce(pForce.force.normalized * (pForce.vectorSum.magnitude - pForce.distance));
                persitentForces.Remove(pForce);
            }
            else
            {
                AddForce(pForce.force * Time.deltaTime);
            }
        }
    }
    protected abstract void Calculate();
}
public struct PersitentForce
{
    public Vector2 force;
    public Vector2 vectorSum;
    public float distance;
    public PersitentForce(Vector2 force ,float distance)
    {
        this.force = force;
        this.distance = distance;
        vectorSum = Vector2.zero;
    }
}

