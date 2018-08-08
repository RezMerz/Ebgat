using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfflineCharacterMove : MonoBehaviour {
    private Vector2 size;
    private Vector2 side;
    private int mask;
    private CharacterAttributes charStats;


    // Use this for initialization
    void Start()
    {
        charStats = GetComponent<CharacterAttributes>();
        size = transform.localScale * GetComponent<BoxCollider2D>().size;
        mask = LayerMask.GetMask("Blocks");
	}
	


    public void MovePressed(int i)
    {
        SpeedCheck(i);
        List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
        bool hit;
        hit = Toolkit.CheckMove(transform.position, size,side, charStats.MoveSpeed * Time.deltaTime, mask , out hitObjects);
        charStats.BodyState = EBodyState.Moving;
        if (!hit)
        {
            transform.position +=  charStats.MoveSpeed * Time.deltaTime * (Vector3)side;
        }
        // hit some objects, move to the nearst
        else
        {
            charStats.ResetMoveSpeed();
            transform.position += (Vector3) side * (hitObjects[0].distance);
        }
    }

    public void MoveReleased()
    {
        charStats.ResetMoveSpeed();
        charStats.BodyState = EBodyState.Standing;
    }

    private void SpeedCheck(int i)
    {
        side = Vector2.right * i;
        if (side != charStats.Side)
        {
            charStats.ResetMoveSpeed();
            charStats.Side = side;
        }
        if (charStats.FeetState == EFeetState.Onground)
        {
            charStats.MoveSpeed  += charStats.MoveAcceleration * Time.deltaTime;
            if (charStats.MoveSpeed > charStats.MoveSpeedMax)
            {
                charStats.MoveSpeed = charStats.MoveSpeedMax;
            }
        }
    }
}
