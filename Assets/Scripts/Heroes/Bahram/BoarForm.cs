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

    private IEnumerator CoolDownTimer(float time)
    {
        yield return new WaitForSeconds(time);
        coolDownLock = false;
        
    }

    private void BoarMoveServerside()
    {
        if (Vector2.Distance(originTransform, transform.position) < range)
        {
            RaycastHit2D hitObject = Physics2D.BoxCast(transform.position, new Vector2(1, 1.8f), 0, charStats.side, speed * Time.deltaTime,layer);
            if (hitObject.collider != null)
            {
                HumanForm();
                transform.position += (hitObject.distance * (Vector3)charStats.side);
                if (hitObject.collider.tag == "Player")
                {
                    hitObject.collider.GetComponent<PlayerControl>().TakeAttack(damage,buff.buffName);
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

    void BoarMoveClientside()
    {
        
    }

    // Boar form is done go back to human
    private void HumanForm()
    {
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
}
