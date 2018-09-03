using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDash : MonoBehaviour
{
    protected CharacterPhysic physic;
    protected float distance;
    protected PlayerControl playerControl;
    protected CharacterAttributes charStats;
    protected Attack attack;
    public float speed;
    public float range;
    public float coolDownTime;
    protected bool coolDownLock;

    protected bool started;

    // Use this for initialization
    protected void Start()
    {
        playerControl = GetComponent<PlayerControl>();
        playerControl.ReadyAction += Initialized;
    }
    private void Initialized()
    {

        charStats = GetComponent<CharacterAttributes>();
        physic = GetComponent<CharacterPhysic>();
        attack = GetComponent<Attack>();
        started = true;
    }


    public virtual void DashPressed()
    {

        if (!coolDownLock)
        {
            if (charStats.Energy >= charStats.dashEnergyConsume && charStats.FeetState != EFeetState.OnWall)
            {

                
                if(charStats.HandState == EHandState.Attacking)
                {
                   attack.IntruptAttack();
                }
                //StartFunction();
                coolDownLock = true;
                StartCoroutine(CoolDownReset());
                charStats.BodyState = EBodyState.Dashing;
                physic.DashLayerSet();
                gameObject.layer = LayerMask.NameToLayer("Dashing");
            }
            else
            {
                print("Low Energy");
            }
        }
    }

    public virtual void DashEnd()
    {
        physic.DashLayerReset();
        gameObject.layer = LayerMask.NameToLayer(charStats.teamName);
        charStats.BodyState = EBodyState.Standing;
        distance = 0;
        StartFunction();
    }
    protected virtual void StartFunction() {}

    protected IEnumerator CoolDownReset()
    {
        yield return new WaitForSeconds(coolDownTime);
        coolDownLock = false;
    }
}
