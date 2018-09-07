using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
public class HeroGraphics : MonoBehaviour
{
    public GameObject landInstance;
    protected SpriteRenderer sprite;
    protected Animator animator;
    protected PlayerControlClientside playerControlClientside;
    protected AudioSource audioSource;
    private Slider hpSlider;
    private Slider energySlider;
    private GameObject hpSliderParent;
    protected Animator abilityEffect;
    private int maxEnergy;
    private float maxHp;
    protected CharacterAim aim;
    protected CharacterAttributesClient charStats;
    protected GameObject HeadIcons;
    protected Color color = Color.white;
    protected HUD hud;
    protected AbilityHudProperty[] abilitiesInfo;
    public GameObject dieInstance;
    private Animator rootMark;
    private GameObject rootGround;
    public GameObject rootGroundInstance;
    float maxRage = 100;
    public void TakeDamage()
    {
        sprite.color = Color.red;
        StartCoroutine(DamageColorTimer(0.1f));
    }


    public void RootMark(string value)
    {
        print(value);
        if(value == "True")
        {
            rootMark.SetBool("Root", true);
        }
        else if (value == "False")
        {
            rootMark.SetBool("Root", false);
        }
    }

    public void Root(string value)
    {
        print(value);
        if (value == "True")
        {
            rootGround = Instantiate(rootGroundInstance);
            rootGround.transform.position = transform.position + new Vector3(0,-0.6f,0);
        }
        else if (value == "False")
        {
            if (rootGround != null)
            {
                rootGround.GetComponent<Animator>().SetTrigger("UnRoot");
                DestoryObjectAfterTime(3, rootGround);
                rootGround = null;
            }
        }
    }

    public void Die()
    {
        GameObject die = Instantiate(dieInstance);
        die.transform.position = transform.position;
        StartCoroutine(DestoryObjectAfterTime(2, die));
    }
    public void SpeedRateChange(string value)
    {
        float speedRate = float.Parse(value);
        if (speedRate < 1)
        {
            sprite.color = new Color(0.27f, 1, 0.952f);
            color = sprite.color;
            animator.speed = speedRate;
        }
        else if(speedRate == 1){
            animator.speed = 1;
            sprite.color = Color.white;
            color = sprite.color;
        }
    }
    IEnumerator DamageColorTimer(float time)
    {
        yield return new WaitForSeconds(time);
        sprite.color = color;
    }

    protected void Start()
    {
        hud = GameObject.FindObjectOfType<HUD>();
        charStats = GetComponent<CharacterAttributesClient>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "AbilityEffect")
                abilityEffect = transform.GetChild(i).GetComponent<Animator>();
            if (transform.GetChild(i).name == "Head Icons")
                HeadIcons = transform.GetChild(i).gameObject;
            if (transform.GetChild(i).name == "Root Alarm")
            {
                rootMark = transform.GetChild(i).GetComponent<Animator>();
            }
        }
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerControlClientside = GetComponent<PlayerControlClientside>();
        GameObject canvas = GameObject.Find("Canvas");
        foreach (Slider slider in canvas.GetComponentsInChildren<Slider>())
        {
            if (slider.name == "Energy Slider")
                energySlider = slider;
        }

        
        aim = GetComponent<CharacterAim>();
        abilitiesInfo = GetComponent<CharacterHudProperty>().abilities;
    }

    public void RageChange(string value)
    {
        float rage = float.Parse(value);
        hud.RageBarFill(rage / maxRage);
    }
    public virtual void AttackNumber(string value)
    {

    }

    protected IEnumerator DestoryObjectAfterTime(float t, GameObject obj)
    {
        yield return new WaitForSeconds(t);
        Destroy(obj);
    }
    public virtual void HandState(string value)
    {

    }
    public virtual void AbilityState(string value)
    {

    }
    public virtual void BodyState(string value)
    {

    }

    public virtual void FeetState(string value)
    {

    }


    public void Disarm(string value)
    {
        if (value == "True")
        {
            HeadIcons.transform.GetChild(1).gameObject.SetActive(true);
        }
        else if (value == "False")
        {
            HeadIcons.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    public void ArmorChange(string value)
    {
        float armor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        if(armor> 0 )
        {
            HeadIcons.transform.GetChild(2).gameObject.SetActive(true);
        }
        else if (armor == 0)
        {
            HeadIcons.transform.GetChild(2).gameObject.SetActive(false);
        }
    }

    public void HpChange(string value)
    {
        float hp = float.Parse(value);
        if (charStats.hp > hp)
            TakeDamage();
        charStats.hp = hp;
        hpSlider.value = hp;
        if (playerControlClientside.IsLocalPlayer())
        {
            hud.HpChange(hp / maxHp);
        }
    }

    public virtual void Aim(string value)
    {
        if(value == "True")
        {
            aim.AimPressedGraphics();
        }
        else if (value == "False")
        {
            aim.AimReleasedGraphic();
        }
    }
    public void EnergyChange(string value)
    {
        float energy = float.Parse(value);
        charStats.energy = (int)energy;
        hud.EnergyCHange(energy / maxEnergy);
    }

    public void BulletShoot(GameObject bullet, Vector2 direction)
    {

    }

    public void CreateHpBar()
    {
        GameObject canvas = GameObject.Find("Canvas");
        GameObject parentInstance = canvas.GetComponent<CanvasManager>().sliderParentInstance;
        hpSliderParent = Instantiate(parentInstance);
        hpSliderParent.transform.SetParent(canvas.transform, false);
        hpSliderParent.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        hpSlider = hpSliderParent.transform.GetChild(0).GetComponent<Slider>();
        hpSlider.value = 1;
        // Change the Color

        if (playerControlClientside.charStatsClient.teamName == PlayerControl.teamName)
        {
            hpSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.green;
        }
        else
        {
            hpSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
        }

        maxHp = charStats.hpBase;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        maxEnergy = charStats.energyBase;
        hud.EnergyCHange(1);
        hud.HpChange(maxEnergy);

    }


    public void HeadState(string value)
    {

        if (value == "1")
            HeadIcons.transform.GetChild(0).gameObject.SetActive(false);
        else if (value == "2")
            HeadIcons.transform.GetChild(0).gameObject.SetActive(true);
    }
    public void ChangePosition(Vector2 pos)
    {
        transform.position = pos;
        hpSliderParent.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public virtual void SetSide(string value)
    {
        Vector2 side = Toolkit.DeserializeVector(value);
        if (side.x == 1)
            sprite.flipX = false;
        else if (side.x == -1)
            sprite.flipX = true;
    }


}
