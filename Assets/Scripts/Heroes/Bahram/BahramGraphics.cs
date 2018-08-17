using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BahramGraphics : HeroGraphics{
    public override void HandState(string value)
    {
        if (value == "2")
            animator.SetTrigger("Attack");

    }

    public override void AbilityState(string value)
    {
        if (value == "1")
        {
            animator.SetBool("Ability1", true);
        }
        else if (value == "2")
        {
            animator.SetBool("Ability1", false);
        }
        else if (value == "3")
        {
            // Ability 2 Start
        }
        else if (value == "4")
        {
            // Ability 2 Finish
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
        else if (value == "5")
            animator.SetTrigger("DoubleJump");
        else
            print("Wrong Feet State Code");

    }
}
