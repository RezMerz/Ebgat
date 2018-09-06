using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroData {
    
    public int playerID { get; private set; }

    private HeroPhysicsClientSide heroPhysics;
    private string data;
    List<string> bullets;
    List<string> bulletHits;
    List<string> additionalData;

    public HeroData(int playerID){
        data = "";
        this.playerID = playerID;
        heroPhysics = new HeroPhysicsClientSide(Vector2.down, Vector2.down);
        bullets = new List<string>();
        bulletHits = new List<string>();
        additionalData = new List<string>();
    }

    public void RegisterCharstat(char keycode, string value){
        data += keycode + "&" + value + "$";
    }

    public void AppendCharstats(string stats)
    {
        data += stats;
    }

    public void RegisterHeroPhysics(Vector2 destination, Vector2 force){
        heroPhysics = new HeroPhysicsClientSide(destination, force);
    }

    public void RegisterBullet(int bulletID, Vector2 attackSide, float gravityAcc,float range,int number,Vector2 startPos,float changeRange){
        bullets.Add(bulletID + "&" + Toolkit.VectorSerialize(attackSide) + "&" + gravityAcc + "&" + range + "&" + number +"&"+ Toolkit.VectorSerialize(startPos)+"&"+changeRange);
    }

    public void RegiterHit(int bulletID){
        bulletHits.Add(bulletID + "");
    }

    public void AdditionalData(string data){
        additionalData.Add(data);
    }

    public string GetData(){
        //Debug.Log(mynum + "    " + data);
        return playerID + "@" + HeroPhysicsClientSide.Serialize(heroPhysics) + "$" + data + "@" + GetBulletData() + "@" + GetBulletHit() + "@" + GetAdditionaldata();
    }

    private string GetBulletData(){
        string tempData = "";
        for (int i = 0; i < bullets.Count; i++){
            tempData += bullets[i] + "$";
        }
        return tempData;
    }

    private string GetBulletHit(){
        string tempData = "";
        for (int i = 0; i < bulletHits.Count; i++)
        {
            tempData += bulletHits[i] + "$";
        }
        return tempData;
    }

    private string GetAdditionaldata(){
        string tempdata = "";
        for (int i = 0; i < additionalData.Count; i++){
            tempdata += additionalData[i] + "$";
        }
        return tempdata;
    }
}
