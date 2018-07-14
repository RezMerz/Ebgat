using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toolkit : MonoBehaviour {
    ///  Vector 2 Transpose
    public static  Vector2 Transpose2(Vector2 vec){
        return new Vector2(vec.y, vec.x);
    }
}
