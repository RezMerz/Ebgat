using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack :Attack {
    public MeleeWaepon weapon;

    public override void AttackPressed()
    {
        
    }

    public override void AttackServerside(Vector2 attackDir)
    {
        Debug.Log("attack server side");
        // Attack Cooldown
        if (cooldownTimer <= 0)
        {
            cooldownTimer = charStats.AttackCooldown;
            playerControl.serverNetworkSender.ClientRangedAttack(playerControl.clientNetworkSender.PlayerID, attackDir);
        }
    }

    public override void AttackClientside(Vector2 attackDir, int attackID)
    {
        Debug.Log("attack client side");
        Vector2 direction = charStats.Side;
        Vector2 origin = (Vector2)transform.position + (direction * (charStats.size / 2));
        heroGraphics.MeleeAttack();
        if (playerControl.IsServer())
            weapon.Attack(origin, charStats.AttackDamage, direction, LayerMask.GetMask(charStats.enemyTeamName), this, attackID);
    }

    public override void AttackHitServerSide(int attackID, float attackDamage, bool hitPlayer)
    {
        Debug.Log("attack hit side");
        playerControl.serverNetworkSender.ClientBulletHit(playerControl.clientNetworkSender.PlayerID, attackID);
        if (hitPlayer)
            playerControl.serverNetworkSender.ClienTakeAttack(playerControl.clientNetworkSender.PlayerID, attackDamage, "");
    }

    public override void AttackHitClientSide(int attackID)
    {
        Debug.Log("attack hit in client");
    }
}
