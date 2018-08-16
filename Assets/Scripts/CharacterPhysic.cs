using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CharacterPhysic : Physic
{
    public Action<List<RaycastHit2D>, List<RaycastHit2D>, Vector2> PhysicAction;
    public HitType hitType;
    private int gravityLayerMask;
    private CharacterAttributes charstats;
    private PlayerControl playerControl;
    private bool layerSet;
    private List<RaycastHit2D> verticalPoints = new List<RaycastHit2D>();
    private List<RaycastHit2D> horizontalPoints = new List<RaycastHit2D>();

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
        virtualPosition = transform.position;
        charstats = GetComponent<CharacterAttributes>();
        start = true;
    }
    private void FixedUpdate()
    {
        if (!start)
            return;

        PhysicAction += HitFunction;
        if (!layerSet)
        {
            layerMask = LayerMask.GetMask("Blocks", charstats.enemyTeamName);
            gravityLayerMask = LayerMask.GetMask("Blocks", "Bridge", charstats.enemyTeamName);
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
            hHit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.right, distance.x, layerMask, out horizontalPoints);
            if (hHit)
            {
                distance.x = horizontalPoints[0].distance;
            }
        }
        else if (distance.x < 0)
        {
            hHit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.left, -distance.x, layerMask, out horizontalPoints);
            if (hHit)
            {
                distance.x = -horizontalPoints[0].distance;
            }
        }
        virtualPosition += Vector2.right * distance;
        if (distance.y > 0)
        {
            vHit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.up, distance.y, layerMask, out verticalPoints);
            if (vHit)
            {
                distance.y = verticalPoints[0].distance;
            }
        }
        else if (distance.y < 0)
        {
            vHit = Toolkit.CheckMoveFloat(virtualPosition, size, Vector2.down, -distance.y, gravityLayerMask, out verticalPoints);
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
        gravityLayerMask = LayerMask.GetMask("Blocks", "Bridge", charstats.enemyTeamName);
        charstats.ResetCayoteTime();
    }
    public void ExcludeBridge()
    {
        gravityLayerMask = layerMask;
        charstats.CayoteTime = 0;
    }
    private void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        if (vHits.Count > 0)
        {
            var hit = vHits[0].collider;
            if (hit.tag == "Player")
            {
                switch (hitType)
                {
                    case HitType.Push:
                        break;
                    case HitType.Throw:
                        hit.gameObject.GetComponent<CharacterPhysic>().AddForce(Vector2.up * (direction.y));
                        break;
                }

            }
        }
        if (hHits.Count > 0)
        {
            var hit = hHits[0].collider;
            if (hit.tag == "Player")
            {
                switch (hitType)
                {
                    case HitType.Push:
                        break;
                    case HitType.Throw:
                        hit.gameObject.GetComponent<CharacterPhysic>().AddForce(Vector2.right * (direction.x));
                        break;
                }
            }
        }

    }
}
public enum HitType { None, Push, Throw }
