using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class king_enemy : EnemyAbstract
{
    public king_enemy()
    {
        attackDamage = 5;
        baseGoldDrop = 8;
        baseHealth = 80f;
        effectResistance = 1.0f;
        speed = 1.0f;
    }
}
