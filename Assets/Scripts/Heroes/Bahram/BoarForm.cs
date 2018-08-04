using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarForm : Ability {
    public float speed;
    public float range;
    private float distanceMoved;
    private Vector2 originTransform;
	// Use this for initialization
	void Start () {
        coolDownLock = false;
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
            transform.position += speed * Time.deltaTime * (Vector3)charStats.side;
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
        distanceMoved = 0;
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
        StartCoroutine(CoolDownTimer(coolDownTime * 100));
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
