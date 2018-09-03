using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmerdadRoot : Ability {

    RootBuff rootBuff;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void AbilityActivateClientSide()
    {
        throw new System.NotImplementedException();
    }

    public override void AbilityKeyHold()
    {
        
    }

    public override void AbilityKeyPrssed()
    {
        GameObject[] playerobj = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerobj.Length; i++){
            PlayerControl playerControl = playerobj[i].GetComponent<PlayerControl>();
            if(!playerControl.charStats.teamName.Equals(charStats.teamName)){
                playerControl.TakeAttack(0, rootBuff.name);
            }
        }
    }

    public override void AbilityKeyReleased()
    {
        
    }
}
