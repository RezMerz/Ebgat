using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousanBullet : Bullet {

    public override void ChangeBehaviour()
    {
        base.ChangeBehaviour();
        physic.SetData(layer + LayerMask.GetMask("Blocks"));
    }
    public override void HitWithSide(Vector2 hitSide)
    {
        shot = false;
        GetComponent<Collider2D>().enabled = false;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.localScale = Vector3.one * 1.5f;
        animator.SetTrigger("Hit");
    }
}
