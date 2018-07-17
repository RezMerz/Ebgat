using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
    [SerializeField]
    private float right_border_;
    [SerializeField]
    private float left_border_;
    [SerializeField]
    private float threshold_;

    private float distance_;
	// Use this for initialization
	void Start ()
    {
        distance_ = right_border_ - left_border_;
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(transform.position.x > right_border_)
        {
            transform.position -= new Vector3 (distance_ + threshold_,0,0);
        }
        if (transform.position.x < left_border_)
        {
            transform.position += new Vector3(distance_ + threshold_, 0, 0);
        }
    }
}
