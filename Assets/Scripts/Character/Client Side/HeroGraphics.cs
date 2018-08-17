using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeroGraphics : MonoBehaviour {
    private SpriteRenderer sprite;
    protected Animator animator;
    private PlayerControl playerControl;
    protected AudioSource audioSource;
    private Slider hpSlider;
    private GameObject hpSliderParent;

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
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerControl = GetComponent<PlayerControl>();
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
        TakeDamage();
        hpSlider.value = float.Parse(value) / 100;
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
    }


    public void HeadState(string value)
    {
        if (value == "1")
            GetComponentInChildren<SpriteRenderer>().enabled = false;
        else if (value =="2")
            GetComponentInChildren<SpriteRenderer>().enabled = true;
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
