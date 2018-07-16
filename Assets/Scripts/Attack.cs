using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour {
    float cooldownTimer;
    private CharacterAttributes charStats;
    public GameObject bulletObj;
	// Use this for initialization
	void Start () {
        charStats = GetComponent<CharacterAttributes>();
        cooldownTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
        if(cooldownTimer>0)
            cooldownTimer -= Time.deltaTime;
        
	}

    public void AttackPressed(Vector2 mousePos)
    {
        // Attack Cooldown
        if (cooldownTimer <= 0)
        {
            cooldownTimer = charStats.attackCooldown;

            if (charStats.attackMode == EAttackMode.Ranged)
                RangeAttack(mousePos);
            else if (charStats.attackMode == EAttackMode.Melee)
                MeleeAttack();
        }
    }

    private void RangeAttack(Vector2 mousePos)
    {
        Vector2 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        GameObject cloneBulletObj = Instantiate(bulletObj);
        cloneBulletObj.GetComponent<Bullet>().Shoot(targetPos, transform.position);

    }

    private void MeleeAttack()
    {
        
    }
}
