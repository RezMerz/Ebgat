using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour {
    private Image hp;
    private Image energy;
    private Image Icon1Black;
    private Image Icon2Black;
    private Image Icon3Black;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Energy")
                energy = transform.GetChild(i).GetComponent<Image>();
            else if (transform.GetChild(i).name == "HP")
                hp = transform.GetChild(i).GetComponent<Image>();
            else if (transform.GetChild(i).name == "Icon 1")
                Icon1Black = transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>();
            else if (transform.GetChild(i).name == "Icon 2")
                Icon2Black = transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>();
            else if (transform.GetChild(i).name == "Icon 3")
                Icon3Black = transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>();
        }
	}
    
    

    public void HpChange(float value)
    {
        hp.fillAmount = value;
    }

    public void EnergyCHange(float value)
    {
        energy.fillAmount = value;
    }

    public void SetImage(Sprite image, int n)
    {
        switch (n)
        {
            case 1: Icon1Black.transform.parent.GetComponent<Image>().sprite = image; break;
            case 2: Icon2Black.transform.parent.GetComponent<Image>().sprite = image; break;
            case 3: Icon3Black.transform.parent.GetComponent<Image>().sprite = image; break;
        }
    }

    public void AbilityStarted()
    {

    }


}
