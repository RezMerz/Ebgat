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

    public override void AttackServerside(Vector2 attackDir)
    {
        // Attack Cooldown
        if (cooldownTimer <= 0)
        {
            cooldownTimer = charStats.AttackCooldown;
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
        Debug.Log(bullets.Count);
        for (int i = 0; i < bullets.Count; i++){
            if(bullets[i].ID == attackID){
                Debug.Log("attackid hit: " + attackID);
                Bullet temp = bullets[i];
                bullets.RemoveAt(i);
                temp.HitClient();
                return;
            }
        }
    }

    public override void AttackHitServerSide(int attackID, float attackDamage, bool hitPlayer)
    {
        playerControl.serverNetworkSender.ClientBulletHit(playerControl.clientNetworkSender.PlayerID, attackID);
        if(hitPlayer)
            playerControl.serverNetworkSender.ClienTakeAttack(playerControl.clientNetworkSender.PlayerID, attackDamage, "");
    }
}
