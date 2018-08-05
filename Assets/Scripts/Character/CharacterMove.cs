using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMove : MonoBehaviour {
    PlayerControl playerControl;
    public float threshold;
    private CharacterAttributes charStats;
    float sizeX;
    float sizeY;
    private Animator animator;
    private Vector2 side;
    private Vector3 destination;
    private int moveSide;
	void Start ()
    {
        animator = GetComponentInChildren<Animator>();
        charStats = GetComponent<CharacterAttributes>();
        playerControl = GetComponent<PlayerControl>();
        destination = transform.position;
        Set_Size(); 
	}

    void Update()
    {
        if (playerControl.IsServer())
        {
            if(charStats.BodyState == EBodyState.Moving)
                MoveServerside();
        }
        else
        {
            if (destination.x - transform.position.x > 0)
                charStats.side = Vector2.right;
            else
                charStats.side = Vector2.left;

            if (Vector3.Distance(transform.position, destination) > Mathf.Epsilon && charStats.BodyState == EBodyState.Moving)
            {
                {
                    float moveDistance = Time.deltaTime * charStats.moveSpeed;
                    float distance = Vector3.Distance(transform.position, destination);


                    if (moveDistance > distance)
                    {
                        transform.position += distance * (Vector3)charStats.side;
                    }
                    else
                    {
                        transform.position += moveDistance * (Vector3)charStats.side;
                    }

             }
        }
           
        }
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
        if (charStats.HeadState != EHeadState.Stunned)
        {
            charStats.BodyState = EBodyState.Moving;
        }
        moveSide = i;
    }
    public void MoveServerside()
    {
        SpeedCheck(moveSide);
        List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
        bool hit;
        hit = Toolkit.CheckMove(transform.position, Get_Size(), Vector2.right * moveSide, charStats.moveSpeed * Time.deltaTime, 256, out hitObjects);
        Vector3 des;
        if (!hit)
        {
            des = transform.position + charStats.moveSpeed * Time.deltaTime * Vector3.right * moveSide;
            transform.position = des;
            playerControl.serverNetworkSender.ClientMove(playerControl.clientNetworkSender.PlayerID, des);
        }
        // hit some objects, move to the nearest
        else
        {
            charStats.ResetMoveSpeed();
            des = transform.position + Vector3.right * moveSide * (hitObjects[0].distance);
            transform.position = des;
            playerControl.serverNetworkSender.ClientMoveFinished(playerControl.clientNetworkSender.PlayerID, des);
        }
        
    }

    public void MoveReleasedServerside(Vector3 position){
        charStats.BodyState = EBodyState.Standing;
        playerControl.serverNetworkSender.ClientMoveFinished(playerControl.clientNetworkSender.PlayerID, position);
    }

    public void MoveReleasedClientside(Vector3 position)
    {
        transform.position = position;
        charStats.BodyState = EBodyState.Standing;
        //animator.SetBool("Walking", false);
    }
    
    public void MoveClientside(Vector2 position)
    {
        if (playerControl.IsServer())
            return;
        charStats.BodyState = EBodyState.Moving;
        destination = position;
        //animator.SetBool("Walking", true);
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

