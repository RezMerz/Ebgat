using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour {
    private Image hp;
    private Image energy;
	// Use this for initialization
	void Start () {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == "Energy")
                energy = transform.GetChild(i).GetComponent<Image>();
            else if (transform.GetChild(i).name == "HP")
                hp = transform.GetChild(i).GetComponent<Image>();
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
}
