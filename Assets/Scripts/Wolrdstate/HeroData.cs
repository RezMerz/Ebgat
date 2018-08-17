﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroData {
    
    public int playerID { get; private set; }

    private HeroPhysicsClientSide heroPhysics;
    private string data;
    List<string> bullets;
    List<string> bulletHits;

    public HeroData(int playerID){
        data = "";
        this.playerID = playerID;
        heroPhysics = new HeroPhysicsClientSide(Vector2.down, Vector2.down);
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

    public void RegisterBullet(int playerID, int bulletID, float damage, Vector2 attackSide, float gravityAcc){
        bullets.Add(playerID + "&" + bulletID + "&" + damage + "&" + attackSide + "&" + gravityAcc);
    }

    public void ResiterHit(int playerID, int bulletID){
        bulletHits.Add(playerID + "&" + bulletID);
    }

    public string GetData(){
        return playerID + "@" + HeroPhysicsClientSide.Serialize(heroPhysics) + "$" + data + "@" + GetBulletData() + "@" + GetBulletHit();
    }

    private string GetBulletData(){
        string data = "";
        for (int i = 0; i < bullets.Count; i++){
            data += bullets[i] + "$";
        }
        return data;
    }

    private string GetBulletHit(){
        string data = "";
        for (int i = 0; i < bulletHits.Count; i++)
        {
            data += bulletHits[i] + "$";
        }
        return data;
    }
}
