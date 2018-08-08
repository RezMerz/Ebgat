using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroPhysicsClientSide
{

    Vector2 destination, force;

    public HeroPhysicsClientSide(Vector2 destination, Vector2 force)
    {
        this.destination = destination;
        this.force = force;
    }

    public static string Serialize(HeroPhysicsClientSide heroPhysicsClientSide)
    {
        return Toolkit.VectorSerialize(heroPhysicsClientSide.destination) + "&" + Toolkit.VectorSerialize(heroPhysicsClientSide.force);
    }

    public static HeroPhysicsClientSide Deserialize(string data){
        string[] parts = data.Split('&');
        return new HeroPhysicsClientSide(Toolkit.DeserializeVector(parts[0]), Toolkit.DeserializeVector(parts[1]));
    }
}
