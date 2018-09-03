using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Globalization;

public class WorldState 
{
    public ServerNetworkSender serverNetworkSender;
    private List<HeroData> heroesData;
    private List<string> additionalData;
   
    public WorldState(){
        heroesData = new List<HeroData>();
        additionalData = new List<string>();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("VirtualPlayer");
        foreach (GameObject obj in objs)
        {
            heroesData.Add(new HeroData(obj.GetComponent<PlayerControl>().playerId));
        }
    }

    private void Refresh(){
        if(heroesData.Count == 0)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("VirtualPlayer");
            Debug.Log(objs.Length);
            foreach (GameObject obj in objs)
            {
                heroesData.Add(new HeroData(obj.GetComponent<PlayerControl>().playerId));
            }
        }
    }

    public void RegisterHeroPhysics(int ID,Vector2 destination, Vector2 force)
    {
        Refresh();
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
        Refresh();
        for (int i = 0; i<heroesData.Count; i++){
            if(ID == heroesData[i].playerID){
                heroesData[i].RegisterCharstat(keycode, value);
                return;
            }
        }

    }

    public void AppendCharstats(int Id, string stats){
        Refresh();
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
        return GetAdditionaldata() + "#" + output;
    }

    public void BulletRegister(int playerID, int bulletID,Vector2 attackSide,float gravityAcc,float range,int attackNumber,Vector2 startPos)
    {
        Refresh();
        for (int i = 0; i < heroesData.Count; i++)
        {
            if (playerID == heroesData[i].playerID)
            {
                heroesData[i].RegisterBullet(bulletID, attackSide, gravityAcc,range,attackNumber,startPos);
                return;
            }
        }
    }

    public void BulletHit(int playerID, int bulletID){
        Refresh();
        for (int i = 0; i < heroesData.Count; i++)
        {
            if (playerID == heroesData[i].playerID)
            {
                heroesData[i].RegiterHit(bulletID);
                return;
            }
        }
    }

    public void AdditionalPlayerData(int playerId, string data){
        Refresh();
        for (int i = 0; i < heroesData.Count; i++){
            if(playerId == heroesData[i].playerID){
                heroesData[i].AdditionalData(data);
                return;
            }
        }   
    }

    public void AdditionalWorldData(string data){
        additionalData.Add(data);
    }

    private string GetAdditionaldata(){
        string output = "";
        for (int i = 0; i < additionalData.Count; i++){
            output += additionalData[i] + "$";
        }
        return output;
    }

    public void print(){
        //Debug.Log(heroesData[0].GetData());
    }
}

public class ObjectData{
    
}