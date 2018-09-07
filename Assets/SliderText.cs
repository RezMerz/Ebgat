using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderText : MonoBehaviour {
    private Slider slider;
    private Text text;
    private CustomNetworkManager customNetworkManager;
	// Use this for initialization
	void Start () {
        slider = transform.parent.GetComponent<Slider>();
        text = GetComponent<Text>();
        customNetworkManager = GameObject.FindWithTag("NetworkManager").GetComponent<CustomNetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = slider.value.ToString();
        customNetworkManager.maxtime = System.Convert.ToInt32(text.text);
	}
}
