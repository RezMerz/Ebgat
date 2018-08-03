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

    private Coroutine moveCycle;
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
        if(Vector3.Distance(transform.position, destination) > Mathf.Epsilon && charStats.BodyState == EBodyState.Moving){
            transform.position += Time.deltaTime * charStats.moveSpeed * (Vector3)charStats.side;
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

    public void MoveRightStart()
    {

    }

    public void MoveLeftStart()
    {

    }
    public void MovePressed(int i)
    {
        MoveServerside(i);
        moveCycle =  StartCoroutine(MoveCycle(0.1f,i));
    }

    private IEnumerator MoveCycle(float time,int i)
    {
        yield return new WaitForSeconds(time);
        MovePressed(i);
    }

    public void MoveServerside(int i)
    {
        SpeedCheck(i);
        List<RaycastHit2D> hitObjects = new List<RaycastHit2D>();
        bool hit;
        hit = Toolkit.CheckMove(transform.position, Get_Size(), Vector2.right * i, charStats.moveSpeed * Time.deltaTime, 256, out hitObjects);
        charStats.BodyState = EBodyState.Moving;
        Vector3 des;
        if (hitObjects.Count == 0)
        {
            des = transform.position + charStats.moveSpeed * Vector3.right * i;
            playerControl.serverNetwork.ClientMove(des);
        }
        // hit some objects, move to the nearst
        else
        {
            print("hit");
            charStats.ResetMoveSpeed();
            des = transform.position + Vector3.right * i * (hitObjects[0].distance);
            
        }
        
    }

    public void MoveReleasedServerside(Vector3 position){
        StopCoroutine(moveCycle);
        MoveReleasedClientside(position);
    }

    public void MoveReleasedClientside(Vector3 position)
    {
        transform.position = position;
        charStats.BodyState = EBodyState.Standing;
        animator.SetBool("Walking", false);
    }
    
    public void MoveClientside(Vector2 position)
    {
        charStats.BodyState = EBodyState.Moving;
        destination = position;
        animator.SetBool("Walking", true);
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

