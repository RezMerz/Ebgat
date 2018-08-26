using System.Collections;
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
            animator.SetBool("Walking", false);
        }
        else if (value == "2")
        {
            animator.SetBool("Walking", true);
        }
        else
            print("Body State Wrong Code");
    }
    public override void FeetState(string value)
    {
        if (value == "1")
            animator.SetTrigger("OnGround");
        else if (value == "2")
            animator.SetTrigger("Fall");
        else if (value == "3")
        {
            audioSource.Play();
            animator.SetTrigger("Jump");
        }
        else if (value == "4")
            print(EFeetState.NoGravity);
        else
            print("Wrong Feet State Code");

    }
}
