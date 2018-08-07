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
	// Use this for initialization
	void Start () {
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

    private void BoarMoveServerside()
    {
        float currentDistance = Toolkit.FloatCut(Time.deltaTime * speed);
        if (currentDistance + distance <= range)
        {
            RaycastHit2D hitObject = Physics2D.BoxCast(transform.position, new Vector2(1, 1.8f), 0, charStats.side, speed * Time.deltaTime,layer);
            if (hitObject.collider != null)
            {
                HumanForm();
                transform.position += (Toolkit.FloatCut(hitObject.distance) * (Vector3)charStats.side);
                if (hitObject.collider.tag == "Player")
                {
                    hitObject.collider.GetComponent<PlayerControl>().TakeAttack(damage,buff.buffName);
                }
            }
            else
            {
                transform.position += Toolkit.FloatCut(speed * Time.deltaTime )* (Vector3)charStats.side;
                distance += Toolkit.FloatCut(speed * Time.deltaTime);
            }
            
        }
        else
        {
            RaycastHit2D hitObject = Physics2D.BoxCast(transform.position, new Vector2(1, 1.8f), 0, charStats.side, range-distance, layer);
            if (hitObject.collider != null)
            {
                HumanForm();
                transform.position += (Toolkit.FloatCut(hitObject.distance) * (Vector3)charStats.side);
                if (hitObject.collider.tag == "Player")
                {
                    hitObject.collider.GetComponent<PlayerControl>().TakeAttack(damage, buff.buffName);
                }
            }
            else
            {
                transform.position += Toolkit.FloatCut(range - distance) * (Vector3)charStats.side;
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
