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
    private bool first;
    private float startTimer;
    protected PlayerControl playerControl;

    // Use this for initialization

    private void LateUpdate()
    {
        if(start)
        {
            if (playerControl != null)
            {
                if (playerControl.IsServer())
                {
                    if (!first)
                    {
                        StartCoroutine(PhysicUpdate());
                        first = true;
                    }
                }
            }
        }
        else{
            startTimer+= Time.deltaTime;
            if(startTimer>1)
                start = true;
        }
    }
    private const double interval = 0.01666;
    private double currentTime;
    public IEnumerator PhysicUpdate()
    {
        while (true)
        {
            if (currentTime < interval)
            {
                currentTime += Time.deltaTime;
                yield return null;
            }
            currentTime = 0;
            Calculate();
        }
    }
    public void AddForce(Vector2 force)
    {
        distance += force;
    }
    protected abstract void Calculate();
}

