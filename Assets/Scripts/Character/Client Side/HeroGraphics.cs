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
        animator.SetBool("Walking", true);
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void MoveLeft()
    {
        print("Move Left");
        animator.SetBool("Walking", true);
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    IEnumerator DamageColorTimer(float time)
    {
        yield return new WaitForSeconds(time);
        sprite.color = playerControl.color;
    }
}
