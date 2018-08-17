using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HeroGraphics : MonoBehaviour {
    private SpriteRenderer sprite;
    private Animator animator;
    private PlayerControl playerControl;
    private AudioSource audioSource;
    private Slider hpSlider;
    private GameObject hpSliderParent;

    public void TakeDamage()
    {
        sprite.color = Color.red;
        StartCoroutine(DamageColorTimer(0.1f));
    }

    public void MeleeAttack()
    {
        animator.SetTrigger("Attack");
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        playerControl = GetComponent<PlayerControl>();
    }
    public void Stand()
    {
        animator.SetBool("Walking", false);
    }
    public void MoveRight()
    {
        print("Move Right");
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void HandState(string value)
    {
        if (value == "2")
            animator.SetTrigger("Attack");
        if(value == "5")
        {

            for(int i=0;i<transform.childCount;i++)
            {
                if (transform.GetChild(i).name == "Glow")
                    transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
        }
        else
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).name == "Glow")
                    transform.GetChild(i).gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    public void AbilityState(string value)
    {
        if(value == "1")
        {
            animator.SetBool("Ability1", true);
        }
        else if( value == "2")
        {
            animator.SetBool("Ability1", false);
        }
        else if (value == "3")
        {
            // Ability 2 Start
        }
        else if (value == "4")
        {
             // Ability 2 Finish
        }
    }
    public void BodyState(string value)
    {
        if (value == "1")
        {
            foreach (Animator childAnim in GetComponentsInChildren<Animator>())
            {
                childAnim.SetBool("Walking", false);
            }
            animator.SetBool("Walking", false);
        }
        else if (value == "2")
        {
            foreach (Animator childAnim in GetComponentsInChildren<Animator>())
            {
                childAnim.SetBool("Walking", true);
            }
            animator.SetBool("Walking", true);
        }
        else
            print("Body State Wrong Code");
    }
    public void MoveLeft()
    {
        print("Move Left");
        animator.SetBool("Walking", true);
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    public void FeetState(string value)
    {
        ResetAnimationParameters();
        if (value == "1")
            animator.SetTrigger("OnGround");
        else if (value == "2")
            animator.SetTrigger("Fall");
        else if (value == "3")
        {
            audioSource.Play();
            animator.SetTrigger("Jump");
        }
        else if (value == "4")
            print(EFeetState.NoGravity);
        else if (value == "5")
            animator.SetTrigger("DoubleJump");
        else
            print("Wrong Feet State Code");
          
    }
    public void HpChange(string value)
    {
        hpSlider.value = int.Parse(value) / 100;
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
    }
    public void ChangePosition(Vector2 pos)
    {
        transform.position = pos;
        hpSliderParent.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void ResetAnimationParameters()
    {
       
    }

    public void SetSide(string value)
    {
        Vector2 side =Toolkit.DeserializeVector(value);
        if (side.x == 1)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (side.x == -1)
            transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    IEnumerator DamageColorTimer(float time)
    {
        yield return new WaitForSeconds(time);
        sprite.color = playerControl.color;
    }
}
