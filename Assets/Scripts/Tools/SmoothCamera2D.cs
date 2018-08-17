using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCamera2D : MonoBehaviour
{
    public float dampTime = 0.15f;
    private Vector3 velocity = Vector3.zero;
    public Transform target;

    public float zoomoutSize;
    public float zoominSize;
    private float targetZoomSize;

    private Vector3 firstPosition;
    private Vector3 targetPos;
    private new Camera camera;

    private bool followTarget;

    private void Start()
    {
        camera = GetComponent<Camera>();
        firstPosition = transform.position;
        targetPos = firstPosition;
        targetZoomSize = zoominSize;
        followTarget = true;
    }

    // Update is called once per frame
    private void  LateUpdate()
    {
        if (followTarget && target)
        {
            Vector3 point = Camera.main.WorldToViewportPoint(target.position);
            Vector3 delta = target.position - Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //(new Vector3(0.5, 0.5, point.z));
            Vector3 destination = transform.position + delta;
            transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);
        }
        else if(transform.position != targetPos)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
        }

        if(target && camera.orthographicSize != targetZoomSize)
        {
            camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetZoomSize,0.1f);
        }
    }

    public void UnfollowTarget()
    {
        followTarget = false;
        targetPos = firstPosition;
        targetZoomSize = zoomoutSize;
    }
    public void FollowTarget()
    {
        followTarget = true;
        targetZoomSize = zoominSize;
    }
}

