using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack {
    
    public GameObject bulletPrefab;
    private List<Bullet> bullets = new List<Bullet>();

    public override void AttackPressed()
    {
        //playerControl.clientNetworkSender.RangedAttack(attackDir);
    }
    public void ShootBullet(Vector3 targetdirection, int attackID)
    {
        GameObject bulletObj = Instantiate(bulletPrefab);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        int layer = LayerMask.GetMask(playerControl.charStats.enemyTeamName, "Blocks");
        bullet.ID = attackID;
        bullets.Add(bullet);
      //  bullet.Shoot(targetdirection, transform.position, layer, playerControl.IsServer(), this);
    }
}
