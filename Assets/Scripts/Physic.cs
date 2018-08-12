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
    protected Vector2 virtualPosition;
    private bool start;
    private float startTimer;
    protected PlayerControl playerControl;

    // Use this for initialization

    private void LateUpdate()
    {
        if(start)
        {
            if(playerControl != null)
            if (playerControl.IsServer() )
                Calculate();
        }
        else{
            startTimer+= Time.deltaTime;
            if(startTimer>1)
                start = true;
        }
    }
    public void AddForce(Vector2 force)
    {
        distance += force;
    }
    protected abstract void Calculate();
}

