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

    public Vector2 virtualPosition { get; protected set;}

    public void AddForce(Vector2 force)
    {
        distance += force;
    }
    protected abstract void Calculate();
}

