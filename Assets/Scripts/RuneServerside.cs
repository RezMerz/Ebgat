using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuneServerside : MonoBehaviour {

    


    public float Hp;
    public Vector2 spawnPosition;

    public void TakeDamge(PlayerControl pl,float damage)
    {
        Hp -= damage;
        if(Hp <= 0)
        {
            LastHit(pl);
        }
    }

    public void LastHit(PlayerControl pl)
    {
        pl.charStats.AddRage( pl.charStats.maxRage - pl.charStats.Rage);
    }

}
