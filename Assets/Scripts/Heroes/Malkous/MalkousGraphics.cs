using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousGraphics : HeroGraphics {

    public GameObject boomEffect;
    public override void HandState(string value)
    {
        if (value == "2")
            animator.SetTrigger("Attack");


    }

    public override void AbilityState(string value)
    {
        print(value);
        if (value == "1")
        {
            GameObject obj = Instantiate(boomEffect, transform.position, Quaternion.Euler(0, 0, 0));
            obj.transform.parent = transform;
            DestoryObjectAfterTime(3,obj);
        }
        else if (value == "2")
        {

        }
        else if (value == "3")
        {
            animator.SetTrigger("Ulti");
            abilityEffect.SetBool("Ulti", true);
        }
        else if (value == "4")
        {
            abilityEffect.SetBool("Ulti", false);
        }
    }
    public override void BodyState(string value)
    {
        aim.AimReleasedGraphic();
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
            animator.SetBool("Dash", true);
        }
        else if (value == "4")
            aim.AimPressedGraphics();
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
      //  else
        //    print("Wrong Feet State Code");

    }
}
