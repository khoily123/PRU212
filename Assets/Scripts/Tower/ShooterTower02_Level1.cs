using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower02_Level1 : ShooterAbstract
{
    public override float ChangeDamageBaseOnLevel()
    {
        return damage * 0.2f;
    }
}

