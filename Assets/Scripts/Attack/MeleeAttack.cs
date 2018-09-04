using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeAttack : Attack
{
    [SerializeField]
    private SwordCombo[] swordCombos;
    [SerializeField]
    private float comboTime;


    private GameObject sword;

    private float damage;
    private Vector2 weaponSize;
    private float distance;
    private string buffName;
    private Vector2 offset;
    private float attackForce;


    private int layerMask;

    //Combo

    private int attackNumber = -1;
    public int maxAttackNumber { get; private set; }
    private Coroutine comboTimeCoroutine;
    private Coroutine parryTimeCoroutine;
    private Coroutine animationTimeCoroutine;
    private new void Start()
    {
        base.Start();
        maxAttackNumber = swordCombos.Length;
        sword = transform.GetChild(0).gameObject;
        sword.layer = gameObject.layer;
        layerMask = LayerMask.GetMask(charStats.enemyTeamName,"Blocks");
        ChangeCombo();
    }

    public override void AttackPressed()
    {
        if (charStats.HeadState != EHeadState.Stunned && charStats.FeetState != EFeetState.OnWall)
        {


            if (ready)
            {
                if (charStats.Energy >= charStats.attackEnergyConsume)
                {
                    if (charStats.BodyState == EBodyState.Dashing)
                    {
                        GetComponent<CharacterDash>().DashEnd();
                    }
                    ChangeCombo();
                    cooldownTimer = charStats.AttackCooldown;
                    charStats.HandState = EHandState.Attacking;
                    if (comboTimeCoroutine != null)
                    {
                        StopCoroutine(comboTimeCoroutine);
                    }
                    animationTimeCoroutine = StartCoroutine(AttackAnimateTime(swordCombos[charStats.AttackNumber].attackAnimationTime / charStats.SpeedRate));
                    ready = false;
                }
                else
                {
                    print("Low Energy");
                }
            }
        }
    }
    private IEnumerator ParryTime()
    {
        sword.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(swordCombos[charStats.AttackNumber].attackAnimationTime * 1f / charStats.SpeedRate);
        sword.GetComponent<BoxCollider2D>().enabled = false;
    }

    protected override void ApplyAttack()
    {

        parryTimeCoroutine = StartCoroutine(ParryTime());
        offset = (charStats.Side + Vector2.up) * offset;

        List<RaycastHit2D> targets = new List<RaycastHit2D>(Physics2D.BoxCastAll(transform.position + (Vector3)offset, weaponSize, 0, charStats.Side, distance, layerMask, 0, 0));
        targets.Sort(new HitDistanceCompare());
        bool parry = false;
        int max = targets.Count;
        for (int i = 0; i < targets.Count; i++)
        {
            if (targets[i].collider.tag.Equals("Block"))
            {
                if (targets[i].point.y - transform.position.y < 2)
                {
                    max = i;
                    float force = (distance - Mathf.Abs((targets[i].point - (Vector2)transform.position).x)) / 10 + attackForce;

                    playerControl.physic.AddReductiveForce(-charStats.Side, force, 0.15f, 0);
                    break;
                }
            }
        }
        for (int i = 0; i < max; i++)
        {
            if (targets[i].collider.tag.Equals("Sword"))
            {
                //Vector2 direction = targets[i].point - (Vector2)transform.positio
                if (targets[i].collider.gameObject.GetComponentInParent<CharacterAttributes>().Side != charStats.Side)
                {
                    if (Toolkit.IsVisible(transform.position, targets[i].point, layerMask, "Sword"))
                    {
                        Debug.Log("parry");
                        parry = true;
                        float force = (distance - Mathf.Abs((targets[i].point - (Vector2)transform.position).x)) / 10 + attackForce;
                        targets[i].collider.gameObject.GetComponentInParent<CharacterPhysic>().AddReductiveForce(charStats.Side, 1.5f * force, 0.3f, 0);
                        playerControl.physic.AddReductiveForce(-charStats.Side, 1.5f * force, 0.3f, 0);
                    }
                }
            }
        }
        if (!parry)
        {
            for (int i = 0; i < max; i++)
            {
                if (targets[i].collider.tag.Equals("VirtualPlayer"))
                {
                    if (Toolkit.IsVisible(transform.position, targets[i].point, layerMask, "VirtualPlayer"))
                    {
                        float force = (distance - Mathf.Abs((targets[i].point - (Vector2)transform.position).x)) / 10 + attackForce;
                        targets[i].collider.gameObject.GetComponent<PlayerControl>().TakeAttack(damage, buffName);
                        targets[i].collider.gameObject.GetComponentInParent<CharacterPhysic>().AddReductiveForce(charStats.Side, force, 0.15f, 0);
                    }
                }
                else if (targets[i].collider.tag.Equals("VirtualBullet"))
                {
                    if (Toolkit.IsVisible(transform.position, targets[i].point, layerMask, "VirtualBullet"))
                    {
                        VirtualBullet bullet = targets[i].collider.gameObject.GetComponent<VirtualBullet>();
                        bullet.Destroy();
                        playerControl.TakeAttack(0, bullet.buff.name);
                    }
                }
            }
        }
        charStats.AttackNumber = (charStats.AttackNumber + 1) % maxAttackNumber;
        if (charStats.AttackNumber != 0)
        {
            comboTimeCoroutine = StartCoroutine(ComboTime());
        }

        StartCoroutine(CoolDown());
    }
    private void ChangeCombo()
    {
        if (attackNumber != charStats.AttackNumber)
        {
            weaponSize = new Vector2(0.1f, swordCombos[charStats.AttackNumber].size.y);
            distance = swordCombos[charStats.AttackNumber].size.x;
            offset = swordCombos[charStats.AttackNumber].offset;
            damage = swordCombos[charStats.AttackNumber].damage;
            attackForce = swordCombos[charStats.AttackNumber].force;
            if (swordCombos[charStats.AttackNumber].buff != null)
            {
                buffName = swordCombos[charStats.AttackNumber].buff.name;
            }
            else
            {
                buffName = "";
            }

            sword.GetComponent<BoxCollider2D>().size = swordCombos[charStats.AttackNumber].size;
            sword.transform.localPosition = (Vector3)charStats.Side * (swordCombos[charStats.AttackNumber].size.x / 2 + offset.x) + Vector3.up * offset.y;

            attackNumber = charStats.AttackNumber;
        }
    }

    public IEnumerator ComboTime()
    {
        yield return new WaitForSeconds(comboTime);
        charStats.AttackNumber = 0;
    }

    public override void IntruptAttack()
    {
        if (animationTimeCoroutine != null)
        {
            StopCoroutine(animationTimeCoroutine);
        }
        if (parryTimeCoroutine != null)
        {
            StopCoroutine(parryTimeCoroutine);
            sword.GetComponent<BoxCollider2D>().enabled = false;
        }
        if (comboTimeCoroutine != null)
        {
            StopCoroutine(comboTimeCoroutine);
        }
        charStats.HandState = EHandState.Idle;
        StartCoroutine(CoolDown());
    }

    public void StartComboCorutine()
    {
        StartCoroutine(ComboTime());
    }
}

[System.Serializable]
public class SwordCombo
{
    public float damage;
    public float attackAnimationTime;
    public Vector2 size;
    public Vector2 offset;
    public float force;
    public Buff buff;
}
