using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroGraphics : MonoBehaviour {
    private SpriteRenderer sprite;

    private PlayerControl playerControl;

    public void TakeDamage()
    {
        sprite.color = Color.red;
        StartCoroutine(DamageColorTimer(0.1f));
    }

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        playerControl = GetComponent<PlayerControl>();
    }

    IEnumerator DamageColorTimer(float time)
    {
        yield return new WaitForSeconds(time);
        sprite.color = playerControl.color;
    }
}
