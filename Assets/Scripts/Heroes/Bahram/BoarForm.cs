using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarForm : Ability {
    public float speed;
    public float range;
    public float stunTime;
    private float distanceMoved;
    private Vector2 originTransform;
    private int layer;
	// Use this for initialization
	void Start () {
        coolDownLock = false;
        layer = LayerMask.GetMask(charStats.enemyTeamName, "Blocks");
	}
	
	// Update is called once per frame
	void Update () {
        if (abilityUse)
        {
            BoarMove();
        }
        
	}

    private IEnumerator CoolDownTimer(float time)
    {
        yield return new WaitForSeconds(time);
        coolDownLock = false;
        
    }

    private void BoarMove()
    {
        if (Vector2.Distance(originTransform,this.transform.position) < range)
        {
            RaycastHit2D hitObject = Physics2D.BoxCast(this.transform.position, new Vector2(1, 1.8f), 0, charStats.side, speed * Time.deltaTime,layer);
            if (hitObject.collider != null)
            {
                HumanForm();
                transform.position += (hitObject.distance * (Vector3)charStats.side);
                if (hitObject.collider.tag == "Player")
                {
                    hitObject.collider.GetComponent<PlayerControl>().TakeStun(stunTime);
                }
            }
            else
            {
                transform.position += speed * Time.deltaTime * (Vector3)charStats.side;
            }
            
        }
        else
        {
            HumanForm();
        }

    }

    // Boar form is done go back to human
    private void HumanForm()
    {
        charStats.HandState = EHandState.Idle;
        charStats.FeetState = EFeetState.Onground;
        abilityUse = false;
    }
    public override void AbilityKeyPrssed()
    {
        if (!coolDownLock)
            StartBoarForm();
    }

    private void StartBoarForm()
    {
        // Remember: Do some code so it can not move or attack
        originTransform = this.transform.position;
        coolDownLock = true;
        StartCoroutine(CoolDownTimer(coolDownTime));
        charStats.HandState = EHandState.Channeling;
        abilityUse = true;
        charStats.FeetState = EFeetState.NoGravity;
        
    }
    public override  void AbilityKeyHold()
    {

    }
    public override void AbilityKeyReleased()
    {

    }
}
