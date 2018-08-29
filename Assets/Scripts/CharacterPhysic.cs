using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterPhysic : Physic
{
    public Action<List<RaycastHit2D>, List<RaycastHit2D>, Vector2> PhysicAction;
    public HitType hitType;
    private int gravityLayerMask;
    private CharacterAttributes charStats;
    private PlayerControl playerControl;
    private bool layerSet;
    private List<RaycastHit2D> verticalPoints = new List<RaycastHit2D>();
    private List<RaycastHit2D> horizontalPoints = new List<RaycastHit2D>();
    private Vector2 offset;
    private float timer;

    private bool start;
    // Use this for initialization
    void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerControl.ReadyAction += Initialize;
    }

    private void Initialize()
    {
        size = transform.localScale * GetComponent<BoxCollider2D>().size;
        offset = transform.localScale * GetComponent<BoxCollider2D>().offset;
        virtualPosition = transform.position;
        charStats = GetComponent<CharacterAttributes>();
        start = true;
    }
    private void FixedUpdate()
    {
        if (!start)
            return;

        CheckPersitentForces();
        PhysicAction += HitFunction;
        if (!layerSet)
        {
            layerMask = LayerMask.GetMask("Blocks", charStats.enemyTeamName);
            gravityLayerMask = LayerMask.GetMask("Blocks", "Bridge", charStats.enemyTeamName);
            layerSet = true;
        }
        Calculate();
    }
    protected override void Calculate()
    {
        bool vHit = false, hHit = false;

        verticalPoints.Clear();
        horizontalPoints.Clear();

        Vector2 originalDistance = distance;
        if (distance.x > 0)
        {
            hHit = Toolkit.CheckMoveFloat(virtualPosition, size, offset * new Vector2(charStats.Side.x, 1), Vector2.right, distance.x, layerMask, out horizontalPoints);
            if (hHit)
            {
                distance.x = horizontalPoints[0].distance;
            }
        }
        else if (distance.x < 0)
        {
            hHit = Toolkit.CheckMoveFloat(virtualPosition, size, offset * new Vector2(charStats.Side.x, 1), Vector2.left, -distance.x, layerMask, out horizontalPoints);
            if (hHit)
            {
                distance.x = -horizontalPoints[0].distance;
            }
        }
        virtualPosition += Vector2.right * distance;
        if (distance.y > 0)
        {
            vHit = Toolkit.CheckMoveFloat(virtualPosition, size, offset * new Vector2(charStats.Side.x, 1), Vector2.up, distance.y, layerMask, out verticalPoints);
            if (vHit)
            {
                distance.y = verticalPoints[0].distance;
            }
        }
        else if (distance.y < 0)
        {
            vHit = Toolkit.CheckMoveFloat(virtualPosition, size, offset * new Vector2(charStats.Side.x, 1), Vector2.down, -distance.y, gravityLayerMask, out verticalPoints);
            if (vHit)
            {
                distance.y = -verticalPoints[0].distance;
            }
        }
        virtualPosition += Vector2.up * distance;
        playerControl.worldState.RegisterHeroPhysics(playerControl.playerId, virtualPosition, distance);
        PhysicAction(verticalPoints, horizontalPoints, originalDistance);
        PhysicAction = null;
        distance = Vector2.zero;
        ServerManager.instance.PlayerSimulationFinished(playerControl.playerId);
    }
    public void IncludeBridge()
    {
        gravityLayerMask = LayerMask.GetMask("Blocks", "Bridge", charStats.enemyTeamName);
        charStats.ResetCayoteTime();
       // RemoveTaggedForces(1);
    }
    public void ExcludeBridge()
    {
        gravityLayerMask = layerMask;
        charStats.CayoteTime = 0;
        //AddPersistentForce(Vector2.down * 20, 1000, 1);
    }
    private void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        StateCheck(direction);

        if (vHits.Count > 0)
        {
            charStats.ResetJumpSpeed();
            charStats.RestJumpMaxSpeed();
            charStats.ResetGravitySpeed();
            RemoveTaggedForces(0);
            //RemoveTaggedForces(1);
            Collider2D hit = vHits[0].collider;
            JumpCheck(hit, direction);
            if (hit.tag.Equals("Player"))
            {
                switch (hitType)
                {
                    case HitType.Throw:
                        hit.gameObject.GetComponent<CharacterPhysic>().AddForce(Vector2.up * (direction.y));
                        break;
                }
            }
        }
        if (hHits.Count > 0)
        {
            OnWallCheck(direction);
            var hit = hHits[0].collider;
            if (hit.tag.Equals("Player"))
            {
                switch (hitType)
                {
                    case HitType.Throw:
                        hit.gameObject.GetComponent<CharacterPhysic>().AddForce(Vector2.right * (direction.x));
                        break;
                }
            }
        }

    }
    private void JumpCheck(Collider2D hit, Vector2 direction)
    {
        if ((hit.tag.Equals("Player") || hit.tag.Equals("jump")) && direction.y < 0)
        {
            AddPersistentForce(Vector2.up *(charStats.JumpSpeed - (direction.y*30)) , 1000, 0);
        }
    }
    private void StateCheck(Vector2 direction)
    {
        if (direction.y > 0)
        {
            if ( charStats.FeetState != EFeetState.WallJumping &&charStats.FeetState != EFeetState.DoubleJumping && charStats.FeetState != EFeetState.Jumping)
            {
                charStats.FeetState = EFeetState.Jumping;
            }
        }
        else
        {
            if (charStats.FeetState != EFeetState.Onground && charStats.FeetState != EFeetState.Falling)
            {
                if(charStats.FeetState == EFeetState.OnWall)
                {
                    timer += Time.deltaTime;
                    if(timer >= 2*  charStats.CayoteTime + Time.deltaTime)
                    {
                        charStats.FeetState = EFeetState.Falling;
                    }
                }
                else
                {
                    charStats.FeetState = EFeetState.Falling;
                }
            }
        }
    }
    private void OnWallCheck(Vector2 direction)
    {
        if ((charStats.FeetState == EFeetState.Falling /* || charStats.FeetState == EFeetState.WallJumping */) && charStats.Side.x * direction.x > 0)
        {
            Debug.Log(charStats.FeetState);
            timer = 0;
            charStats.FeetState = EFeetState.OnWall;
            RemoveTaggedForces(3);
        }
    }


}
public enum HitType { None, Push, Throw }
