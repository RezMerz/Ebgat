﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Globalization;

public class WorldState 
{
    public ServerNetworkSender serverNetworkSender;
    List<HeroData> heroesData;
    List<string> bullets;
    List<string> bulletHits;
   
    public WorldState(){
        heroesData = new List<HeroData>();
        bullets = new List<string>();
        bulletHits = new List<string>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject obj in objs)
            heroesData.Add(new HeroData(obj.GetComponent<PlayerControl>().playerId));
    }

    public void RegisterHeroPhysics(int ID,Vector2 destination, Vector2 force){
        

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
        for (int i = 0; i<heroesData.Count; i++){
            if(ID == heroesData[i].playerID){
                heroesData[i].RegisterCharstat(keycode, value);
                return;
            }
        }

    }

    public void AppendCharstats(int Id, string stats){
        for (int i = 0; i < heroesData.Count; i++)
        {
            if (Id == heroesData[i].playerID)
            {
                heroesData[i].AppendCharstats(stats);
                return;
            }
        }
    }

    public string GetWorldData(){
        string output = "";
        for (int i = 0; i < heroesData.Count; i++){
            output += heroesData[i].GetData() + "#";
        }
        return output;
    }

    public void BulletRegister(int playerID, int bulletID,float damage,Vector2 attackSide,float gravityAcc)
    {
        bullets.Add(playerID + "&" + bulletID + "&" +damage + "&" +attackSide + "&" +gravityAcc);
    }

    public void BulletHit(int playerID, int bulletID){
        bulletHits.Add(playerID + "&" + bulletID);
    }
}

public class ObjectData{
    
}