using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterTower02_Level3 : ShooterAbstract
{
    public override float ChangeDamageBaseOnLevel()
    {
        return damage;
    }
}
