using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Globalization;

public class UpdatableWorldState {
    public ServerNetworkSender serverNetworkSender;
    List<HeroData> heroesData;
    List<string> bullets;
    List<string> bulletHits;

    public UpdatableWorldState()
    {
        heroesData = new List<HeroData>();
        bullets = new List<string>();
        bulletHits = new List<string>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in objs)
            heroesData.Add(new HeroData(obj.GetComponent<PlayerControl>().clientNetworkSender.PlayerID));
    }

    public void RegisterHeroPhysics(int ID, Vector2 destination, Vector2 force)
    {

        //Debug.Log(ID);
        for (int i = 0; i < heroesData.Count; i++)
        {
            if (ID == heroesData[i].playerID)
            {
                heroesData[i].RegisterHeroPhysics(destination, force);
                return;
            }
        }
    }

    public void RegisterCharStat(int ID, char keycode, string value)
    {
        /*if(heroesData.Count == 0)
        {
            Debug.Log("frameeeesasasae: " + Time.frameCount);
            GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in objs)
                heroesData.Add(new HeroData(obj.GetComponent<PlayerControl>().clientNetworkSender.PlayerID));
        }*/

        for (int i = 0; i < heroesData.Count; i++)
        {
            if (ID == heroesData[i].playerID)
            {
                heroesData[i].RegisterCharstat(keycode, value);
                return;
            }
        }

    }

    public string[] GetWorldData()
    {
        string[] output = new string[heroesData.Count];
        for (int i = 0; i < heroesData.Count; i++)
        {
            output[i] = heroesData[i].GetData() + "$" + Time.frameCount;
        }
        return output;
    }

    public void BulletRegister(string playerID, string bulletID)
    {
        bullets.Add(playerID + "&" + bulletID);
    }

    public void BulletHit(string playerID, string bulletID)
    {
        bulletHits.Add(playerID + "&" + bulletID);
    }
}
