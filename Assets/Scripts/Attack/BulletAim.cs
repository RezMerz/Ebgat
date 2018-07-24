using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletAim : MonoBehaviour {
    private Bullet bullet;
    private LineRenderer line;

    private Vector2 targetPos;

	// Use this for initialization
	void Start ()
    {
        bullet = GetComponent<Bullet>();
        line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
