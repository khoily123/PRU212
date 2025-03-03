using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower01_Level3 : ShooterAbstract
{
    public override float ChangeDamageBaseOnLevel()
    {
        return damage * 2.0f;
    }
}
