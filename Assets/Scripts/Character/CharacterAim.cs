using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAim : MonoBehaviour {
    CharacterAttributes charStats;
    PlayerControl playerControl;
    private GameObject arrow;
    private Vector2 _centre;
    private float angle;
    private float radius = 3;
    private Vector2 position;
    private int n = 8;
	// Use this for initialization
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        charStats = playerControl.charStats;
        for(int i =0;i<transform.childCount;i++)
        {
            if (transform.GetChild(i).tag == "AimUI")
                arrow = transform.GetChild(i).gameObject;
        }
    }



    public void AimPressed()
    {
        charStats.BodyState = EBodyState.Aiming;
        Cursor.visible = false;
        angle = 0;
        position = new Vector2(1, 0);
        arrow.SetActive(true);
        
    }

    public void AimReleased()
    {
        charStats.BodyState = EBodyState.Standing;
        Cursor.visible = true;
        arrow.SetActive(false);
        charStats.AimSide = charStats.Side;
    }

    public void yChange(float deltaY)
    {
        if (deltaY > 0 && position.y < radius)
            position += new Vector2(0, deltaY);
        else if (deltaY < 0 && position.y > -radius)
            position += new Vector2(0, deltaY);
        ChangeRotation();
        
    }

    public void XChange(float deltaX)
    {
        if (deltaX > 0 && position.x < radius)
            position += new Vector2(deltaX, 0);
        else if (deltaX < 0 && position.x > -radius)
            position += new Vector2(deltaX, 0);

     
        ChangeRotation();
    }

    public void ControllerAim(Vector2 aimAxis)
    {
        if (aimAxis != position)
        {
            position = aimAxis;
            ChangeRotation();
        }
    }

    private void ChangeRotation()
    {
        angle = Vector2.SignedAngle(Vector2.right, position);
        if (position.x < 0)
            charStats.Side = new Vector2(-1, 0);
        else if (position.x > 0)
            charStats.Side = new Vector2(1, 0);
        float rotation = Mathf.Ceil(angle / (360 / n)) * 360 / n;
       arrow.transform.rotation = Quaternion.Euler(0, 0, rotation);
       charStats.AimSide = new Vector2(Mathf.Cos(rotation * Mathf.Deg2Rad), Mathf.Sin(rotation * Mathf.Deg2Rad));
    }

    
}
