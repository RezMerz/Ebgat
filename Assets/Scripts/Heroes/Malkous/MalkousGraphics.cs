﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousGraphics : HeroGraphics {

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

        }
        else if (value == "2")
        {

        }
        else if (value == "3")
        {
            animator.SetBool("Ulti", true);
            abilityEffect.SetBool("Ulti", true);
        }
        else if (value == "4")
        {
            animator.SetBool("Ulti", false);
            abilityEffect.SetBool("Ulti", false);
        }
    }
    public override void BodyState(string value)
    {
        if (value == "1")
        {
            animator.SetBool("Dash", false);
            animator.SetBool("Walking", false);
        }
        else if (value == "2")
        {
            animator.SetBool("Dash", false);
            animator.SetBool("Walking", true);
        }
        else if(value == "3")
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Dash",true);
        }
        else
            print("Body State Wrong Code");
    }
    public override void FeetState(string value)
    {
        animator.SetBool("OnWall", false);
        if (value == "1")
        {
            GameObject land = Instantiate(landInstance);
            StartCoroutine(DestoryObjectAfterTime(1, land));
            land.transform.position = transform.position + Vector3.down * 3 /2;
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
        else
            print("Wrong Feet State Code");

    }
}
