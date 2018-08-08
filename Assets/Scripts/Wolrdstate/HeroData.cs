using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroData {
    
    public int playerID { get; private set; }

    private HeroPhysicsClientSide heroPhysics;
    private string data;

    public HeroData(int playerID){
        data = "";
        this.playerID = playerID;
    }

    public void RegisterCharstat(char keycode, string value){
        data += keycode + "&" + value;
    }

    public void RegisterHeroPhysics(Vector2 destination, Vector2 force){
        heroPhysics = new HeroPhysicsClientSide(destination, force);
    }

    public string GetData(){
        return HeroPhysicsClientSide.Serialize(heroPhysics) + "$" + data;
    }
}
