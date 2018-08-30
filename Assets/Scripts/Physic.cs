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

    public Vector2 virtualPosition { get; set; }

    public void AddForce(Vector2 force)
    {
        distance += force;
    }
    public void AddPersistentForce(Vector2 force, float distance, int removeTag)
    {
        persitentForces.Add(new PersitentForce(force, distance, removeTag));
    }

    public void RemoveTaggedForces(int tag)
    {
        for (int i = 0; i < persitentForces.Count; i++)
        {
            var force = persitentForces[i];
            if (force.removeTag == tag)
            {
                persitentForces.Remove(force);
            }
        }
    }

    protected void CheckPersitentForces()
    {
        for (int i = 0; i < persitentForces.Count; i++)
        {
            persitentForces[i].vectorSum += persitentForces[i].force * Time.deltaTime;
            if (persitentForces[i].distance != 0 && persitentForces[i].vectorSum.magnitude > persitentForces[i].distance)
            {
                AddForce(persitentForces[i].force.normalized * (persitentForces[i].vectorSum.magnitude - persitentForces[i].distance));
                persitentForces.Remove(persitentForces[i]);

            }
            else
            {
                AddForce(persitentForces[i].force * Time.deltaTime);
            }
        }
    }
    protected abstract void Calculate();
}
public class PersitentForce
{
    public Vector2 force;
    public Vector2 vectorSum;
    public float distance;
    public int removeTag;
    public PersitentForce(Vector2 force, float distance, int removeTag)
    {
        this.force = force;
        this.distance = distance;
        this.removeTag = removeTag;
        vectorSum = Vector2.zero;
    }

}

