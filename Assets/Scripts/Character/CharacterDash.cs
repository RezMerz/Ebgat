using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDash : MonoBehaviour {
    protected CharacterPhysic physic;
    protected float distance;

    protected CharacterAttributes charStats;
    public float speed;
    public float range;
    public float coolDownTime;
    protected bool coolDownLock;
	// Use this for initialization
	void Start () {
        physic = GetComponent<CharacterPhysic>();
        charStats = GetComponent<CharacterAttributes>();
	}


    public void DashPressed()
    {
       
       if(!coolDownLock)
       {
           print("Dash Pressed");
           coolDownLock = true;
           StartCoroutine(CoolDownReset());
           charStats.BodyState = EBodyState.Dashing;
       }
    }

    private IEnumerator CoolDownReset(){
        yield return new WaitForSeconds(coolDownTime);
        coolDownLock = false;
    }
}
