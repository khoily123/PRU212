using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpamerLV2 : SpammerAbstract
{
   void Awake()
    {
        spawnRate = 1.0f;
        maxEnemies = 15;
        maxWaves = 6;
    }
}
