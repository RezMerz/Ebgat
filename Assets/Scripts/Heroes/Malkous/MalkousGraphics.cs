using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousGraphics : HeroGraphics {
    private int attackNumber = 0;
    public GameObject boomEffect;
    public override void HandState(string value)
    {
        if (value == "2")
            animator.SetTrigger("Attack");


    }
    public override void AttackNumber(string value)
    {
        attackNumber = int.Parse(value);
        animator.SetInteger("Attack Number", attackNumber);
    }
    public override void AbilityState(string value)
    {
        print(value);
        if (value == "1")
        {
            GameObject obj = Instantiate(boomEffect, transform.position, Quaternion.Euler(0, 0, 0));
            obj.transform.parent = transform;
            DestoryObjectAfterTime(3,obj);
            if(playerControlClientside.IsLocalPlayer())
                hud.AbilityStarted(2, abilitiesInfo[1].cooldown);
        }
        else if (value == "2")
        {

        }
        else if (value == "3")
        {
            animator.SetTrigger("Ulti");
        }
        else if (value == "4")
        {
            abilityEffect.SetBool("Ulti", false);
        }
    }
    public override void BodyState(string value)
    {
        gameObject.layer = LayerMask.NameToLayer(charStats.teamName);
        animator.SetBool("Walking", false);
        if (value == "1")
        {
            animator.SetBool("Dash", false);
        }
        else if (value == "2")
        {
            animator.SetBool("Dash", false);
            animator.SetBool("Walking", true);
        }
        else if (value == "3")
        {
            if(playerControlClientside.IsLocalPlayer())
                hud.AbilityStarted(1, abilitiesInfo[0].cooldown);
            animator.SetBool("Dash", true);
            gameObject.layer = LayerMask.NameToLayer("Dashing");
        }
        else
            print("Body State Wrong Code");
    }
    public override void FeetState(string value)
    {
        animator.SetBool("OnWall", false);
        if (value == "1")
        {
            // GameObject land = Instantiate(landInstance);
            // StartCoroutine(DestoryObjectAfterTime(1, land));
            // land.transform.position = transform.position + Vector3.down * 3 /2;
            animator.SetTrigger("OnGround");
        }
        else if (value == "2")
            animator.SetTrigger("Fall");
        else if (value == "3")
        {
            audioSource.Play();
            animator.SetTrigger("Jump");
        }
        else if (value == "6")
        {
            animator.SetBool("OnWall", true);
        }
        else if (value == "4")
            print(EFeetState.NoGravity);
        else if (value == "8")
            animator.SetTrigger("Jump");

    }
}
