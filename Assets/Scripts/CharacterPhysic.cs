using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterPhysic : Physic
{
    public float fallDamage;
    public float spikeDamage;

    public Action<List<RaycastHit2D>, List<RaycastHit2D>, Vector2> PhysicAction;
    public HitType hitType;
    private int gravityLayerMask;
    private int wallLayerMask;
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


        virtualPosition = transform.position;
        CheckPersitentForces();
        CheckReductiveForces();
        PhysicAction += HitFunction;
        if (!layerSet)
        {
            layerMask = LayerMask.GetMask("Blocks", charStats.enemyTeamName);
            gravityLayerMask = LayerMask.GetMask("Blocks", "Bridge", charStats.enemyTeamName);
            wallLayerMask = LayerMask.GetMask("Blocks");
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
        transform.position = virtualPosition;
        playerControl.worldState.RegisterHeroPhysics(playerControl.playerId, virtualPosition, distance);
        PhysicAction(verticalPoints, horizontalPoints, originalDistance);
        PhysicAction = null;
        distance = Vector2.zero;
        ServerManager.instance.PlayerSimulationFinished(playerControl.playerId);
    }
    public void IncludeBridge()
    {
        gravityLayerMask += LayerMask.GetMask("Bridge");
        charStats.ResetCayoteTime();
    }
    public void ExcludeBridge()
    {
        gravityLayerMask -= LayerMask.GetMask("Bridge");
        charStats.CayoteTime = 0;
    }

    public void DashLayerSet()
    {
        layerMask -= LayerMask.GetMask(charStats.enemyTeamName);
        gravityLayerMask -= LayerMask.GetMask(charStats.enemyTeamName);
    }
    public void DashLayerReset()
    {
        layerMask += LayerMask.GetMask(charStats.enemyTeamName);
        gravityLayerMask += LayerMask.GetMask(charStats.enemyTeamName);
    }
    private void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        StateCheck(direction);

        if (vHits.Count > 0)
        {
            charStats.ResetJumpSpeed();
            charStats.ResetJumpMaxSpeed();
            charStats.ResetGravitySpeed();
            RemoveTaggedForces(0);
            Collider2D hit = vHits[0].collider;
            JumpCheck(hit, direction);
            SpikeHitVertical(hit, direction);
            if (hit.tag.Equals("VirtualPlayer"))
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
            SpikeHitHorizantal(hHits[0], direction);
            OnWallCheck(direction);
            var hit = hHits[0].collider;
            if (hit.tag.Equals("VirtualPlayer"))
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
        Vector2 force;
        if (direction.y < 0)
        {
            if (hit.tag.Equals("VirtualPlayer"))
            {
                force = Vector2.up * (charStats.JumpSpeed - direction.y * 20);
                hit.gameObject.GetComponent<PlayerControl>().TakeAttack(fallDamage, "");
                AddPersistentForce(force, 0, 0);
            }
            else if (hit.tag.Equals("jump"))
            {
                force = Vector2.up * (charStats.JumpSpeed - direction.y * 40);
                AddPersistentForce(force, 0, 0);
            }
        }
    }
    private void StateCheck(Vector2 direction)
    {
        if (direction.y > 0)
        {
            if (charStats.FeetState != EFeetState.WallJumping && charStats.FeetState != EFeetState.DoubleJumping && charStats.FeetState != EFeetState.Jumping)
            {
                charStats.FeetState = EFeetState.Jumping;
            }
        }
        else
        {
            if (charStats.FeetState != EFeetState.Onground && charStats.FeetState != EFeetState.Falling)
            {
                if (charStats.FeetState == EFeetState.OnWall)
                {
                    timer += Time.deltaTime;
                    if (timer >= 0.12f + Time.deltaTime)
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
        if ((charStats.FeetState == EFeetState.Falling || charStats.FeetState == EFeetState.OnWall) && charStats.Side.x * direction.x > 0)
        {
            if (Toolkit.OnWallCheck(virtualPosition, size, offset * new Vector2(charStats.Side.x, 1), charStats.Side, Mathf.Abs(direction.x), wallLayerMask))
            {
                timer = 0;
                charStats.FeetState = EFeetState.OnWall;
                charStats.wallside = (int)charStats.Side.x;
                RemoveTaggedForces(3);
                RemoveTaggedForces(4);
            }
        }
    }

    private void SpikeHitVertical(Collider2D hit, Vector2 direction)
    {
        if (hit.tag.Equals("Spike"))
        {
            if (charStats.BodyState == EBodyState.Dashing)
            {
                GetComponent<CharacterDash>().DashEnd();
            }
            if (direction.y > 0)
            {
                AddReductiveForce(Vector2.down, 0.6f, 0.1f, 0);
            }
            else
            {
                Vector2 force = Vector2.up * (charStats.JumpSpeed - direction.y * 15);
                AddPersistentForce(force, 0, 0);
            }
            playerControl.TakeAttack(spikeDamage, "");
        }
    }

    private void SpikeHitHorizantal(RaycastHit2D hit, Vector2 direction)
    {
        if (hit.collider.tag.Equals("Spike"))
        {
            if (charStats.BodyState == EBodyState.Dashing)
            {
                GetComponent<CharacterDash>().DashEnd();
            }

            Vector2 hitDirection;
            if (direction.x > 0)
            {
                hitDirection = Vector2.right;
            }
            else
            {
                hitDirection = Vector2.left;
            }
            playerControl.TakeAttack(spikeDamage, "");
            AddReductiveForce(hitDirection * -1, 0.7f, 0.1f, 0);
        }
    }


}
public enum HitType { None, Push, Throw }
