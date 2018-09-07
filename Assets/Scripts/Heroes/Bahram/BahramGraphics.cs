using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BahramGraphics : HeroGraphics{
    public GameObject IronFistInstance;
    private int attackNumber = 1;
    void Start()
    {
        base.Start();
        hud.SetBlack(5,0);
    }
    public override void HandState(string value)
    {
        if (value == "2")
        {
            animator.SetInteger("Attack Number", attackNumber);
            animator.SetTrigger("Attack");
        }

    }

    private void AttackEffect1()
    {

    }

    private void AttackEffect2()
    {

    }
    public override void AttackNumber(string value)
    {
        attackNumber = int.Parse(value) +1 ;
    }

    public override void AbilityState(string value)
    {
        if (value == "1")
        {
            animator.SetBool("Ability1", true);
            if(playerControlClientside.IsLocalPlayer())
                hud.AbilityStarted(2, abilitiesInfo[1].cooldown);
        }
        else if (value == "2")
        {
            animator.SetBool("Ability1", false);
            GameObject ironFistEffect = Instantiate(IronFistInstance, transform.position, Quaternion.Euler(0, 0, 0));
            StartCoroutine(DestoryObjectAfterTime(1.5f, ironFistEffect));
        }
        else if (value == "3")
        {
            animator.SetBool("Boar", true);
        }
        else if (value == "4")
        {
            animator.SetBool("Boar", false);
        }
        else if (value == "5")
        {
            animator.SetTrigger("Ability 3");
        }
    }
    public override void BodyState(string value)
    {
        if (charStats.bodyState == EBodyState.Dashing)
            if (value == "1")
                animator.SetTrigger("Idle");
        animator.SetBool("Walking", false);
        gameObject.layer = LayerMask.NameToLayer(charStats.teamName);
        if (value == "1")
        {

        }
        else if (value == "2")
        {

            animator.SetBool("Walking", true);
        }
        else if(value == "3")
        {
            gameObject.layer = LayerMask.NameToLayer("Dashing");
            if(playerControlClientside.IsLocalPlayer())
                hud.AbilityStarted(1, abilitiesInfo[0].cooldown);
            animator.SetTrigger("Roll");
        }
        else
            print("Body State Wrong Code");
    }
    public override void FeetState(string value)
    {
        hud.SetBlack(5, 0);
        animator.SetBool("OnWall", false);
        if (value == "1")
        {
            GameObject land = Instantiate(landInstance);
            StartCoroutine(DestoryObjectAfterTime(1, land));
            land.transform.position = transform.position + Vector3.down * 3 / 2;
            if (charStats.bodyState != EBodyState.Dashing)
                animator.SetTrigger("OnGround");
        }
        else if (value == "2")
        {
            animator.SetTrigger("Fall");
        }
        else if (value == "3")
        {
            animator.SetTrigger("Jump");
        }
        else if (value == "6")
        {
            animator.SetBool("OnWall", true);
        }
        else if (value == "4")
        {
            //print(EFeetState.NoGravity);
        }
        else if (value == "5")
        {
            animator.SetTrigger("DoubleJump");
            hud.SetBlack(5, 1);
        }
        else if (value == "8")
            animator.SetTrigger("Jump");

    }
}
