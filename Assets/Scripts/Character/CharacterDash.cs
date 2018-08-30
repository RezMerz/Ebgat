using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDash : MonoBehaviour {
    protected CharacterPhysic physic;
    protected float distance;
    protected PlayerControl playerControl;
    protected CharacterAttributes charStats;
    public float speed;
    public float range;
    public float coolDownTime;
    protected bool coolDownLock;
	// Use this for initialization
	void Start () {
        physic = GetComponent<CharacterPhysic>();
        charStats = GetComponent<CharacterAttributes>();
        playerControl = GetComponent<PlayerControl>();
	}


    public void DashPressed()
    {
       
       if(!coolDownLock)
       {
           if (charStats.Energy >= charStats.dashEnergyConsume)
           {
               coolDownLock = true;
               StartCoroutine(CoolDownReset());
               charStats.BodyState = EBodyState.Dashing;
           }
           else
           {
               print("Low Energy");
           }
       }
    }

    private IEnumerator CoolDownReset(){
        yield return new WaitForSeconds(coolDownTime);
        coolDownLock = false;
    }
}
