using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdatableHeroData : MonoBehaviour {

    public int playerID { get; private set; }

    private HeroPhysicsClientSide heroPhysics;
    private string data;
    private List<string> dataList;

    public UpdatableHeroData(int playerID)
    {
        data = "";
        this.playerID = playerID;
        dataList = new List<string>();
        heroPhysics = new HeroPhysicsClientSide(Vector2.down, Vector2.down);
    }

    public void RegisterCharstat(char keycode, string value)
    {
        dataList.Add(keycode + "&" + value + "$");
    }

    public void RegisterHeroPhysics(Vector2 destination, Vector2 force)
    {
        heroPhysics = new HeroPhysicsClientSide(destination, force);
    }

    public string GetData()
    {
        foreach (string s in dataList)
        {
            data += s;
        }
        return playerID + "$" + HeroPhysicsClientSide.Serialize(heroPhysics) + "$" + data;
    }
}
