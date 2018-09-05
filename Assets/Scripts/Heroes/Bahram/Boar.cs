using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Ability
{
    [SerializeField]
    private float duration;
    [SerializeField]
    private float minMovementSpeed;
    [SerializeField]
    private float maxMovementSpeed;
    [SerializeField]
    private float movementAcceleration;
    [SerializeField]
    private float minJumpSpeed;
    [SerializeField]
    private float maxJumpSpeed;
    [SerializeField]
    private float jumpAcceleration;
    [SerializeField]
    private float gravityAcceleration;
    [SerializeField]
    private float onWallGravitySpeed;


    private float minMoveSpeedHuman;
    private float maxMoveSpeedHuman;
    private float moveAccelerationHuman;

    private float minJumpSpeedHuman;
    private float maxJumpSpeedHuman;
    private float jumpAccelerationHuman;

    private float gravityAccelerationHuman;
    private float onWallGravitySpeedHuman;

    private CharacterPhysic physic;
    private BuffManager buffManager;
    void Start()
    {
        buffManager = GetComponent<BuffManager>();
        physic = GetComponent<CharacterPhysic>();

        // Save Orignal Attributes
        minMoveSpeedHuman = charStats.moveSpeedBase;
        maxMoveSpeedHuman = charStats.moveSpeedMaxBase;
        moveAccelerationHuman = charStats.moveAccelerationBase;

        minJumpSpeedHuman = charStats.jumpAccelerationBase;
        maxJumpSpeedHuman = charStats.jumpSpeedMaxBase;
        jumpAccelerationHuman = charStats.jumpAccelerationBase;

        gravityAccelerationHuman = charStats.gravityAccelerationBase;
        onWallGravitySpeedHuman = charStats.onWallGravitySpeed;
    }
    public override void AbilityKeyPrssed()
    {
        if (charStats.HeadState == EHeadState.Conscious && (charStats.FeetState == EFeetState.Onground || charStats.FeetState == EFeetState.Root ) )
        {
            if (!coolDownLock)
            {
                if (energyUsage >= charStats.Energy)
                {
                    coolDownLock = true;

                    physic.Lock();
                    castTimeCoroutine = StartCoroutine(CastTime(castTime));
                    charStats.HandState = EHandState.Casting;
                    charStats.AbilityState = EAbility.Ability2Start;
                }
            }
        }
    }
    protected override void AbilityCast()
    {
        charStats.HandState = EHandState.Idle;
        physic.Unlock();
        StartCoroutine(Duration());
        StartCoroutine(CoolDownTimer(coolDownTime));

        BoarForm();
    }
    private void BoarForm()
    {
        buffManager.DebuffAllCharacter();

        charStats.moveSpeedBase = minMovementSpeed;
        charStats.moveSpeedMaxBase = maxMovementSpeed;
        charStats.moveAccelerationBase = movementAcceleration;

        charStats.jumpSpeedBase = minJumpSpeed;
        charStats.jumpSpeedMaxBase = maxJumpSpeed;
        charStats.jumpAccelerationBase = jumpAcceleration;
        charStats.canDoubleJump = false;

        charStats.gravityAccelerationBase = gravityAcceleration;
        charStats.onWallGravitySpeed = onWallGravitySpeed;

        charStats.ResetMoveSpeed();
        charStats.ResetMoveSpeedMax();
        charStats.ResetMoveAcceleration();

        charStats.ResetJumpSpeed();
        charStats.ResetJumpSpeed();
        charStats.ResetMoveAcceleration();
        charStats.ResetGravitAcceleration();


        charStats.AttackNumber = 3;
    }
    private void HumanForm()
    {
        buffManager.DebuffAllCharacter();

        charStats.moveSpeedBase = minMoveSpeedHuman;
        charStats.moveSpeedMaxBase = maxMoveSpeedHuman;
        charStats.moveAccelerationBase = moveAccelerationHuman;

        charStats.jumpSpeedBase = minJumpSpeedHuman;
        charStats.jumpSpeedMaxBase = maxJumpSpeedHuman;
        charStats.jumpAccelerationBase = jumpAccelerationHuman;
        charStats.canDoubleJump = true;

        charStats.gravityAccelerationBase = gravityAccelerationHuman;
        charStats.onWallGravitySpeed = onWallGravitySpeedHuman;

        charStats.ResetMoveSpeed();
        charStats.ResetMoveSpeedMax();
        charStats.ResetMoveAcceleration();

        charStats.ResetJumpSpeed();
        charStats.ResetJumpSpeed();
        charStats.ResetMoveAcceleration();
        charStats.ResetGravitAcceleration();

        charStats.AttackNumber = 0;
    }
    private IEnumerator Duration()
    {
        yield return new WaitForSeconds(duration);
        GetComponent<PlayerJump>().IntruptJump();
        GetComponent<BahramDash>().DashEnd();
        GetComponent<MeleeAttack>().IntruptAttack();
        charStats.AbilityState = EAbility.Ability2Finish;
        StartCoroutine(BacKToHumanForm());
    }
    private IEnumerator BacKToHumanForm()
    {
        charStats.HandState = EHandState.Casting;
        physic.Lock(); 
        yield return new WaitForSeconds(castTime);
        charStats.HandState = EHandState.Idle;
        physic.Unlock();
        HumanForm();
    }
    protected override void IntruptCast()
    {
        if(castTimeCoroutine != null)
        {
            StopCoroutine(castTimeCoroutine);
            physic.Unlock();
        }
    }
    // Unused
    public override void AbilityKeyHold()
    {

    }
    public override void AbilityKeyReleased()
    {

    }
    public override void AbilityActivateClientSide()
    {
        throw new System.NotImplementedException();
    }
}
