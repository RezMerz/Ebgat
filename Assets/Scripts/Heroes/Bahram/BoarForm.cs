using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarForm : Ability {
    public float speed;
    public float range;
    public float damage;
    private float distanceMoved;
    private Vector2 originTransform;
    private int layer;
    private PlayerControl playerControl;
    private float distance;
    private CharacterPhysic physic;
	// Use this for initialization
	void Start () {
        physic = GetComponent<CharacterPhysic>();
        coolDownLock = false;
        layer = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
        playerControl = GetComponent<PlayerControl>();
	}
	
	// Update is called once per frame
	void Update () {
        if (abilityUseServerside)
        {
            BoarMoveServerside();
        }
        else if(abilityUseClientside)
        {
            
        }
        
	}
    private void BoarMoveHitFunction(List<RaycastHit2D> vHits, List<RaycastHit2D> hHits, Vector2 direction)
    {
        HumanForm();
        if (vHits.Count > 0 && vHits[0].collider.tag == "Player")
        {
            vHits[0].collider.GetComponent<PlayerControl>().TakeAttack(damage, buff.name);
        }
        else if (hHits.Count > 0 && hHits[0].collider.tag == "Player")
            hHits[0].collider.GetComponent<PlayerControl>().TakeAttack(damage, buff.name);
    }
    private void BoarMoveServerside()
    {
        float currentDistance = Toolkit.FloatCut(Time.deltaTime * speed);
        physic.AddForce(charStats.Side * speed * Time.deltaTime);
        physic.PhysicAction += BoarMoveHitFunction;

        if (currentDistance + distance <= range)
        {

                transform.position += Toolkit.FloatCut(speed * Time.deltaTime )* (Vector3)charStats.Side;
                distance += Toolkit.FloatCut(speed * Time.deltaTime);
            
        }
        else
        {
            RaycastHit2D hitObject = Physics2D.BoxCast(transform.position, new Vector2(1, 1.8f), 0, charStats.Side, range-distance, layer);
            if (hitObject.collider != null)
            {
                HumanForm();
                transform.position += (Toolkit.FloatCut(hitObject.distance) * (Vector3)charStats.Side);
                if (hitObject.collider.tag == "Player")
                {
                    hitObject.collider.GetComponent<PlayerControl>().TakeAttack(damage, buff.buffName);
                }
            }
            else
            {
                transform.position += Toolkit.FloatCut(range - distance) * (Vector3)charStats.Side;
                distance += Toolkit.FloatCut(range - distance);
            }
            HumanForm();
        }
    }

    void BoarMoveClientside()
    {
        
    }

    // Boar form is done go back to human
    private void HumanForm()
    {
        print(distance);
        charStats.HandState = EHandState.Idle;
        charStats.FeetState = EFeetState.Onground;
        abilityUseServerside = false;
    }
    public override void AbilityKeyPrssedServerSide()
    {
        if (!coolDownLock)
            StartBoarFormServerside();
    }

    private void StartBoarFormServerside()
    {
        distance = 0;
        // Remember: Do some code so it can not move or attack
        originTransform = transform.position;
        coolDownLock = true;
        StartCoroutine(CoolDownTimer(coolDownTime));
        charStats.HandState = EHandState.Channeling;
        abilityUseServerside = true;
        charStats.FeetState = EFeetState.NoGravity;
        
    }

    public void StartBoarFormClientSide(Vector2 originPosition){
        originTransform = originPosition;
        coolDownLock = true;
        StartCoroutine(CoolDownTimer(coolDownTime));
        charStats.HandState = EHandState.Channeling;
        abilityUseServerside = true;
        charStats.FeetState = EFeetState.NoGravity;
    }

    public override  void AbilityKeyHold()
    {

    }
    public override void AbilityKeyReleased()
    {

    }

    public override void AbilityActivateClientSide()
    {
        throw new System.NotImplementedException();
    }
}
