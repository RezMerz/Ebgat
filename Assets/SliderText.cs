using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderText : MonoBehaviour {
    private Slider slider;
    private Text text;
	// Use this for initialization
	void Start () {
        slider = transform.parent.GetComponent<Slider>();
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = slider.value.ToString();
	}
}
