using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack {
    
    public GameObject bulletPrefab;
    private List<Bullet> bullets = new List<Bullet>();

    public override void AttackPressed()
    {
    }
}
