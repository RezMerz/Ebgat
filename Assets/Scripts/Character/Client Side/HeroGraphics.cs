using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeroGraphics : MonoBehaviour {
    public GameObject landInstance;
    private SpriteRenderer sprite;
    protected Animator animator;
    private PlayerControl playerControl;
    protected AudioSource audioSource;
    private Slider hpSlider;
    private Slider energySlider;
    private GameObject hpSliderParent;
    protected Animator abilityEffect;
    private int maxEnergy;
    private float maxHp;
    protected CharacterAim aim;
    protected CharacterAttributesClient charStats;
    
    public void TakeDamage()
    {
        sprite.color = Color.red;
        StartCoroutine(DamageColorTimer(0.1f));
    }

    IEnumerator DamageColorTimer(float time)
    {
        yield return new WaitForSeconds(time);
        sprite.color = Color.white;
    }

    void Start()
    {
        
        charStats = GetComponent<CharacterAttributesClient>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).tag == "AbilityEffect")
                abilityEffect = transform.GetChild(i).GetComponent<Animator>();
        }
            audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerControl = GetComponent<PlayerControl>();
        GameObject canvas = GameObject.Find("Canvas");
        foreach(Slider slider in canvas.GetComponentsInChildren<Slider>())
        {
            if (slider.name == "Energy Slider")
                energySlider = slider;
        }

       
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

    
    public void HpChange(string value)
    {
        float hp = float.Parse(value);
        if(charStats.hp > hp)
            TakeDamage();
        charStats.hp = hp;
        hpSlider.value = hp;
    }

    public void EnergyChange(string value)
    {
        float energy = float.Parse(value);
        charStats.energy = (int)energy;
        energySlider.value = energy;
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
        if (playerControl.IsServer())
        {
            if (playerControl.charStats.teamName == PlayerControl.teamName)
            {
                hpSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.green;
            }
            else
            {
                hpSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
            }
        }
        else
        {


            if (playerControl.charStatsClient.teamName == PlayerControl.teamName)
            {
                hpSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.green;
            }
            else
            {
                hpSlider.transform.GetChild(1).GetChild(0).GetComponent<Image>().color = Color.red;
            }
        }

        maxHp = charStats.hpBase;
        hpSlider.maxValue = maxHp;
        hpSlider.value = maxHp;
        maxEnergy = charStats.energyBase;
        energySlider.maxValue = maxEnergy;
        energySlider.value = maxEnergy;

    }


    public void HeadState(string value)
    {
        if (value == "1")
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
        else if (value == "2")
            transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
    }
    public void ChangePosition(Vector2 pos)
    {
        transform.position = pos;
        hpSliderParent.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void SetSide(string value)
    {
        Vector2 side =Toolkit.DeserializeVector(value);
        if (side.x == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (side.x == -1)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }


}
