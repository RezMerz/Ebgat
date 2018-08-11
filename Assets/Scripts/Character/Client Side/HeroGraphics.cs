using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroGraphics : MonoBehaviour {
    private SpriteRenderer sprite;
    private Animator animator;
    private PlayerControl playerControl;

    public void TakeDamage()
    {
        sprite.color = Color.red;
        StartCoroutine(DamageColorTimer(0.1f));
    }

    public void MeleeAttack()
    {
        animator.SetTrigger("Attack");
    }
    void Start()
    {
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerControl = GetComponent<PlayerControl>();
    }
    public void Stand()
    {
        animator.SetBool("Walking", false);
    }
    public void MoveRight()
    {
        print("Move Right");
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }
    public void BodyState(string value)
    {
        if (value == "1")
            animator.SetBool("Walking", false);
        else if (value == "2")
            animator.SetBool("Walking", true);
        else
            print("Body State Wrong Code");
    }
    public void MoveLeft()
    {
        print("Move Left");
        animator.SetBool("Walking", true);
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void FeetState(string value)
    {
        if(value == "1") 
            print(EFeetState.Onground);
        else if( value == "2")
            print(EFeetState.Falling);
        else if(value == "3")
            print(EFeetState.Jumping);
        else if(value == "4")
            print(EFeetState.NoGravity);
        else if(value == "5")
            print(EFeetState.DoubleJumping);
        else
            print("Wrong Feet State Code");
    }

    public void SetSide(string value)
    {
        Vector2 side =Toolkit.DeserializeVector(value);
        if (side.x == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (side.x == -1)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    IEnumerator DamageColorTimer(float time)
    {
        yield return new WaitForSeconds(time);
        sprite.color = playerControl.color;
    }
}
