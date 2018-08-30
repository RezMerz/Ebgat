using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAim : MonoBehaviour {
    CharacterAttributes charStats;
    PlayerControl playerControl;
	// Use this for initialization
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
    }

    private Vector2 _centre;
    private float angle;
    private float radius;
    private Vector2 position;
    public void Aim()
    {
        angle = 0;
        
    }
    public void yChange(float deltaY)
    {
        position += new Vector2(0, deltaY * 10);
        Vector2 arrow = position - (Vector2)transform.position;
        
    }

    public void XChange(float deltaX)
    {
        position += new Vector2(deltaX * 10, 0);
    }

    
}
