using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {
    private CharacterAttributes charStats;
    private Attack attack;
	// Use this for initialization
	void Start () {
        charStats = GetComponent<CharacterAttributes>();
     
        }

    // Some Damage has been done
    public void TakeAttack(float damage,Buff buff)
    {
        if (buff != null)
        {
            // buff code here
        }
        TakeDamage(damage);

    }

    private void TakeDamage(float damage)
    {
        print("Took Damage");
        charStats.hitPoints -= damage;
        if (charStats.hitPoints <= 0)
        {
            //Death
            print("Dead");
        }
    }
}


