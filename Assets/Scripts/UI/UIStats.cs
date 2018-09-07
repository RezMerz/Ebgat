using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIStats : MonoBehaviour {
    private static float timer;
    private float time = 210;
    private Text timeText;
    private bool redTimeLock;
    private bool urgentTimeLock;
    private Text team1Kills;
    private Text team2Kills;
    private int redTime = 1;
    private int urgentTime = 15;
    private static Text kill1;
    private static Text kill2;
	// Use this for initialization
	void Start () {
        timer = time;
        timeText = transform.GetChild(3).GetComponent<Text>();
        kill1 = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        kill2 = transform.GetChild(1).GetChild(0).GetComponent<Text>();
	}
    public static void SetTime(int t){
        timer = t;
    }

    public static void SetKill(int team,int number)
    {
        if (team == 1)
            kill1.text = number.ToString();
        else if (team == 2)
            kill2.text = number.ToString();
    }

    public void SetKills(int teamNumber,int n)
    {
        if(teamNumber == 1)
        {
            team1Kills.text = n.ToString();
        }
        else if (teamNumber == 2)
        {
            team2Kills.text = n.ToString();
        }
        else
        {
            print(" Wrong Team Number");
        }
    }
	// Update is called once per frame
	void Update () {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            int minutes = (int)timer / 60;
            if(minutes < redTime  && !redTimeLock)
            {
                redTimeLock = true;
                timeText.color = Color.red;
                print(" Warning: One Minute Remaining");
            }
            int seconds = (int)timer % 60;
            if(minutes < redTime && seconds < urgentTime && !urgentTimeLock)
            {
                urgentTimeLock = true;
                timeText.GetComponent<Animator>().SetBool("Urgent", true);
            }
            string mString = minutes.ToString();
            string sString = seconds.ToString();
            if (seconds < 10)
                sString = "0" + sString;
            timeText.text = mString + " : " + sString;
        }
        else if (timer < 0)
        {
            timeText.GetComponent<Animator>().SetBool("Urgent", false);

        }
	}
}
