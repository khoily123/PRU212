using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower05_Level2 : ShooterAbstract
{
    public override float ChangeDamageBaseOnLevel()
    {
        return damage * 1.5f;
    }
}
