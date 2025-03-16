using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblinRider : EnemyAbstract
{
    public goblinRider()
    {
        baseHealth = 30f;
        baseGoldDrop = 3;
        attackDamage = 3;
        speed = 3.0f;
    }
}

