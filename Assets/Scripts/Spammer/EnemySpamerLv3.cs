using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySpamerLv3 : SpammerAbstract
{
    void Awake()
    {
        spawnRate = 1.5f;
        maxEnemies = 10;
        maxWaves = 5;
    }
}
