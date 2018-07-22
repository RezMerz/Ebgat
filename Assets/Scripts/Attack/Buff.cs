using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff : MonoBehaviour {
    //Buff attributes
    public CharacterAttributes charStats{get;set;}
    public string name;
    public float time;
    public float timer;
    public bool stackable;
    
    //health attributes
    public float deltaArmor;
    public float deltaHp;


    //attack attributes
    public float deltaDamage;
    public float deltaAttackCooldown;
    public float deltaAttackAnimation;
    public float deltaAttackRange;


    //Move attributes
    public float deltaMoveSpeed;
    public float deltaMoveAcceleration;
    public float deltaMoveSpeedMax;


    // Jump Attributes
    public float deltaJumpSpeed;
    public float deltaJumpAcceleration;
    public float deltaJumpSpeedMax;


    //gravity attributes
    public float deltaGravitySpeed;
    public float deltaGravityAcceleration;
    public float deltaGravitySpeedMax;

    private bool finish;

    void Start()
    {

        finish = false;
    }

    public void BuffCharacter()
    {
        timer = time;
        charStats.armor += deltaArmor;
        charStats.hitPoints += deltaHp;
        charStats.attackCooldown += deltaAttackCooldown;
        charStats.attackAnimationTime += deltaAttackAnimation;
        charStats.range += deltaAttackRange;
        charStats.moveSpeed += deltaMoveSpeed;
        charStats.moveAcceleration += deltaMoveAcceleration;
        charStats.moveSpeedMax += deltaMoveSpeedMax;
        charStats.jumpAcceleration += deltaJumpAcceleration;
        charStats.jumpSpeed += deltaJumpSpeed;
        charStats.jumpSpeedMax += deltaJumpSpeedMax;
        charStats.gravityAcceleration += deltaGravityAcceleration;
        charStats.gravitySpeed += deltaGravitySpeed;
        charStats.gravitySpeedMax += deltaGravitySpeedMax;
    }

    public void DebuffCharacter()
    {
        charStats.armor -= deltaArmor;
        charStats.hitPoints -= deltaHp;
        charStats.attackCooldown -= deltaAttackCooldown;
        charStats.attackAnimationTime -= deltaAttackAnimation;
        charStats.range -= deltaAttackRange;
        charStats.moveSpeed -= deltaMoveSpeed;
        charStats.moveAcceleration -= deltaMoveAcceleration;
        charStats.moveSpeedMax -= deltaMoveSpeedMax;
        charStats.jumpAcceleration -= deltaJumpAcceleration;
        charStats.jumpSpeed -= deltaJumpSpeed;
        charStats.jumpSpeedMax -= deltaJumpSpeedMax;
        charStats.gravityAcceleration -= deltaGravityAcceleration;
        charStats.gravitySpeed -= deltaGravitySpeed;
        charStats.gravitySpeedMax -= deltaGravitySpeedMax;
    }
	// Use this for initialization
    void Update()
    {
        if (!finish)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            else
            {
                finish = true;
                DebuffCharacter();
                GameObject.Destroy(gameObject);
            }
        }
    }
}
