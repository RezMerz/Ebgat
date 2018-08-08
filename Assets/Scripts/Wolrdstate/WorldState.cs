using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class WorldState
{
    public ServerNetworkSender serverNetworkSender;
    List<HeroData> heroesData;

    public void RegisterHeroPhysics(int ID,Vector2 destination, Vector2 force){
        if (!serverNetworkSender.isServer)
            return;
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
        if (!serverNetworkSender.isServer)
            return;
        for (int i = 0; i<heroesData.Count; i++){
            if(ID == heroesData[i].playerID){
                heroesData[i].RegisterCharstat(keycode, value);
                return;
            }
        }
    }

    public void UpdatePlayerCount(List<PlayerControl> playerControls)
    {
        for (int i = 0; i < playerControls.Count; i++){
            for (int j = 0; j < heroesData.Count; j++){
                if (playerControls[i].clientNetworkSender.PlayerID == heroesData[j].playerID)
                    continue;
            }
            heroesData.Add(new HeroData(playerControls[i].clientNetworkSender.PlayerID));
        }
    }

    public string[] GetWorldData(){
        string[] output = new string[heroesData.Count];
        for (int i = 0; i < heroesData.Count; i++){
            output[i] = heroesData[i].GetData();
        }
        return output;
    }
}

public class ObjectData{
    
}