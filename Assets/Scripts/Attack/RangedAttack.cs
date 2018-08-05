using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : Attack {
    
    public GameObject bulletPrefab;
    private List<Bullet> bullets = new List<Bullet>();

    public override void AttackPressed(Vector2 attackDir)
    {
        playerControl.clientNetworkSender.RangedAttack(attackDir);
    }

    public override void AttackServerside(Vector2 attackDir)
    {
        // Attack Cooldown
        if (cooldownTimer <= 0)
        {
            cooldownTimer = charStats.attackCooldown;
            playerControl.serverNetworkSender.ClientRangedAttack(playerControl.clientNetworkSender.PlayerID, attackDir);
        }
    }

    public override void AttackClientside(Vector2 attackDir, int attackID)
    {
        ShootBullet(attackDir, attackID);
    }

    public void ShootBullet(Vector3 targetdirection, int attackID)
    {
        GameObject bulletObj = Instantiate(bulletPrefab);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        int layer = LayerMask.GetMask(playerControl.charStats.enemyTeamName, "Blocks");
        bullet.ID = attackID;
        bullets.Add(bullet);
        bullet.Shoot(targetdirection, transform.position, layer, playerControl.IsServer(), this);
    }

    public override void AttackHitClientSide(int attackID)
    {
        Debug.Log("Hit id: " + attackID);
        for (int i = 0; i < bullets.Count; i++){
            Debug.Log("bullet id: " + attackID);
            if(bullets[i].ID == attackID){
                Debug.Log("attackid hit: " + attackID);
                bullets[i].HitClient();
            }
        }
    }

    public override void AttackHitServerSide(int attackID)
    {
        playerControl.serverNetworkSender.ClientBulletHit(attackID);
    }
}
