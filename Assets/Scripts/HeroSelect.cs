using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroSelect : MonoBehaviour {
    private Animator animator;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
	}

    public void HeroShow(int value)
    {
        print(value);
        GameManager.instance.playerConnection.CmdSetMyHero(GameManager.instance.playerConnection.clientId, value);
        animator.SetInteger("Hero", value);
    }
}
