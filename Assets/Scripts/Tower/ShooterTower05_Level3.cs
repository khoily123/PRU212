using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower05_Level3 : ShooterAbstract
{
    public override float ChangeDamageBaseOnLevel()
    {
        return damage * 2.0f;
    }
}
