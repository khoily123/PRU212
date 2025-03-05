using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower02_Level2 : ShooterAbstract
{
    public override float ChangeDamageBaseOnLevel()
    {
        return damage * 0.4f;
    }
}

