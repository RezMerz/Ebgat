using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera_Follow : MonoBehaviour {
    [SerializeField]
    private GameObject player_;
    [SerializeField]
    private float smoothness_;

    private Vector3 distance_;
	// Use this for initialization
	void Start ()
    {
        distance_ = transform.position - player_.transform.position;	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = Vector3.Lerp(transform.position,player_.transform.position+distance_, smoothness_);
	}
}
