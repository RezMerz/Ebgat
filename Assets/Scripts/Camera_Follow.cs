using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Camera_Follow : MonoBehaviour {
    
    [SerializeField]
    public GameObject player_;
    [SerializeField]
    private float smoothness_;

    private Vector3 distance_;

    private bool follow;
	// Use this for initialization
	void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(!follow)
            return;
        transform.position = Vector3.Lerp(transform.position,player_.transform.position+distance_, smoothness_);
	}

    public void Startfollowing(){
        distance_ = transform.position - player_.transform.position;    
        follow = true;
    }
}
