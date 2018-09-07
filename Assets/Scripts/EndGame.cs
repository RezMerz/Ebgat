using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndGame : MonoBehaviour {
    private Text text;
	// Use this for initialization
	void Start () {
        text = transform.GetChild(2).GetComponentInChildren<Text>();
	}


    public void EndGameFunction(int n)
    {
        if (n == 0)
            text.text = "Draw!";
        else if (n == 1)
            text.text = "Team 1 Wins";
        else if (n == 2)
            text.text = "Team 2 Wins";
    }

}
