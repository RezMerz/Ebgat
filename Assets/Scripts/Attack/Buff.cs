using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Buff : MonoBehaviour {
    //Buff attributes
    public CharacterAttributes charStats{get;set;}
    public string buffName;
    public float time;
    public float timer;
    public bool stackable;
    
    

    public bool finish;

    public void FinishBuff()
    {
        finish = true;
        DebuffCharacter();
        GameObject.Destroy(gameObject);
    }

    void Start()
    {
        finish = false;
    }

    public abstract void BuffCharacter();

    public abstract void DebuffCharacter();

	// Use this for initialization
    void Update()
    {
        if (!finish)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                FinishBuff();
            }
        }
    }
}
