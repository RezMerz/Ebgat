using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterMove : MonoBehaviour {
    

    private PlayerControl playerControl;
    private CharacterAttributes charStats;
    private CharacterPhysic Physic;
    private Animator animator;
    private Vector2 side;
    private Vector2 size;
    private Vector3 destination;
    private int moveSide;
    private int layerMask;
	void Start ()
    {
        Physic = GetComponent<CharacterPhysic>();
        animator = GetComponentInChildren<Animator>();
        charStats = GetComponent<CharacterAttributes>();
        playerControl = GetComponent<PlayerControl>();
        destination = transform.position;
        size = transform.localScale * GetComponent<BoxCollider2D>().size;

        layerMask = LayerMask.GetMask("Blocks", charStats.enemyTeamName);
	}

    void Update()
    {
        if (playerControl.IsServer())
        {
            if(charStats.BodyState == EBodyState.Moving)
                MoveServerside();
        }
        //else
        //{
        //    float distance = destination.x - transform.position.x;
        //    if (distance > 0)
        //        charStats.side = Vector2.right;
        //    else
        //        charStats.side = Vector2.left;

        //    if (charStats.BodyState == EBodyState.Moving && Mathf.Abs(distance) > Mathf.Epsilon)
        //    {
        //        {
        //            float moveDistance = Time.deltaTime * charStats.moveSpeed;

        //            if (moveDistance > Mathf.Abs(distance))
        //            {
        //                transform.position += Mathf.Abs(distance) * (Vector3)charStats.side;
        //            }
        //            else
        //            {
        //                transform.position += moveDistance * (Vector3)charStats.side;
        //            }
        //        }
        //    }
           
        //}
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
        Physic.AddForce(Vector2.right * moveSide * charStats.moveSpeed * Time.deltaTime);
        Physic.PhysicAction += HitFunction;
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
    private void HitFunction(List<RaycastHit2D> vHits,List<RaycastHit2D> hHits,Vector2 direction)
    {
        if(direction.x * moveSide > 0 && hHits.Count > 0)
        {
            charStats.ResetMoveSpeed();
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

