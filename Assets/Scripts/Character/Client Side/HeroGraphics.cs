using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
public class HeroGraphics : MonoBehaviour
{
    public GameObject landInstance;
    private SpriteRenderer sprite;
    protected Animator animator;
    private PlayerControlClientside playerControlClientside;
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
    private HUD hud;

    public void TakeDamage()
    {
        sprite.color = Color.red;
        StartCoroutine(DamageColorTimer(0.1f));
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

    void Start()
    {

        charStats = GetComponent<CharacterAttributesClient>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "AbilityEffect")
                abilityEffect = transform.GetChild(i).GetComponent<Animator>();
            if (transform.GetChild(i).name == "Head Icons")
                HeadIcons = transform.GetChild(i).gameObject;
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

        hud = GameObject.FindObjectOfType<HUD>();
        aim = GetComponent<CharacterAim>();
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


    public void ArmorChange(string value)
    {
        float armor = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
        if(armor> 0 )
        {
            HeadIcons.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (armor == 0)
        {
            HeadIcons.transform.GetChild(0).gameObject.SetActive(false);
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

    public void SetSide(string value)
    {
        Vector2 side = Toolkit.DeserializeVector(value);
        if (side.x == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (side.x == -1)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }


}
