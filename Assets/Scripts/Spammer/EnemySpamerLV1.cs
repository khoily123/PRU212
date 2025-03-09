using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpamerLV1 : SpammerAbstract
{
   void Awake()
    {
        spawnRate = 1.0f;
        maxEnemies = 10;
        maxWaves = 5;
    }
}
