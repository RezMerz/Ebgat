using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MalkousanVirtual : VirtualBullet
{

    public override void ChangeBehaviour()
    {
        base.ChangeBehaviour();
        physic.SetData(layer + LayerMask.GetMask("Blocks"));
    }
}
