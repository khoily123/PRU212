using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower01_Level1 : ShooterAbstract
{
    public override float ChangeDamageBaseOnLevel()
    {
        return damage;
    }
}
