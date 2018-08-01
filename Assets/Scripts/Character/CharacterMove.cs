using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMove : MonoBehaviour {
    public float threshold;
    private CharacterAttributes charStats;
    float sizeX;
    float sizeY;
    private Animator animator;

    private Vector2 side;

	void Start ()
    {
        animator = GetComponentInChildren<Animator>();
        charStats = GetComponent<CharacterAttributes>();
        Set_Size();
	}
    void Set_Size()
    {
        sizeX = transform.localScale.x * GetComponent<BoxCollider2D>().size.x;
        sizeY = transform.localScale.y * GetComponent<BoxCollider2D>().size.y;
    }
    public Vector2 Get_Size()
    {
        return new Vector2(sizeX, sizeY);
    }
    
    // ray cast for move in square objects

    public void MovePressed(int i)
    {
        SpeedCheck(i);
        List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
        bool hit;
        hit = Toolkit.CheckMove(transform.position, Get_Size(), Vector2.right * i, charStats.moveSpeed * Time.deltaTime, 256,out hitObjects);
        charStats.BodyState = EBodyState.Moving;
        animator.SetBool("Walking", true);
        Move(Vector2.right * i, charStats.moveSpeed * Time.deltaTime, hitObjects);
        
    }

    public void MoveReleased()
    {
        animator.SetBool("Walking", false);
    }
    
    private void Move(Vector2 direction, float distance, List<RaycastHit2D> hitObjects)
    {
        // hit nothing , move at distance
        if(hitObjects.Count == 0)
        {
            transform.position += distance * (Vector3)direction;
        }
        // hit some objects, move to the nearst
        else
        {
            charStats.ResetMoveSpeed();
            transform.position += (Vector3)direction * (hitObjects[0].distance);
        }
    }
    private void SpeedCheck(int i)
    {
        side = Vector2.right * i;
        if(side  != charStats.side)
        {
            charStats.ResetMoveSpeed();
            charStats.side = side;
        }
        if(charStats.BodyState == EBodyState.Standing)
        {
            charStats.ResetMoveSpeed();
        }
        if(charStats.BodyState == EBodyState.Moving && charStats.FeetState == EFeetState.Onground)
        {
            charStats.moveSpeed += charStats.moveAcceleration;
            if(charStats.moveSpeed > charStats.moveSpeedMax)
            {
                charStats.moveSpeed = charStats.moveSpeedMax;
            }
        }
    }

}


public class HitDistanceCompare : IComparer<RaycastHit2D>
{
    public int Compare(RaycastHit2D x, RaycastHit2D y)
    {
        if (x.distance == y.distance)
            return 0;
        if (x.distance < y.distance)
            return -1;
        else return 1;
    }
}

