using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterPhysic : Physic {
    public HitType hitType;
    private CharacterAttributes charstats;
    private Gravity gravity;

    private bool layerSet;



	// Use this for initialization
	void Start ()
    {
        layerSet = true;
        size = transform.localScale * GetComponent<BoxCollider2D>().size;
        virtualPosition = transform.position;
        playerControl = GetComponent<PlayerControl>();
        charstats = GetComponent<CharacterAttributes>();
    }

    private void Update()
    {
        if (layerSet)
        {
            layerMask = LayerMask.GetMask("Blocks", charstats.enemyTeamName);
            Debug.Log(charstats.enemyTeamName);
            Debug.Log(LayerMask.GetMask(charstats.enemyTeamName));
            gravityLayerMask = LayerMask.GetMask("Blocks", "Bridge", charstats.enemyTeamName);
            layerSet = false;
        }
    }

    public void IncludeBridge()
    {
        gravityLayerMask = LayerMask.GetMask("Blocks", "Bridge",charstats.enemyTeamName);
        charstats.ResetCayoteTime();
    }
    public void ExcludeBridge()
    {
        gravityLayerMask = layerMask;
        charstats.CayoteTime = 0;
    }
    protected override void HitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        if(vHits.Count> 0)
        {
            var hit = vHits[0].collider;
            if(hit.tag == "Player")
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
public enum HitType {None,Push,Throw}
