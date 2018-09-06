using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet {


    public override void HitWithSide(Vector2 hitSide)
    {
        shot = false;
        GetComponent<Collider2D>().enabled = false;
    }
}
