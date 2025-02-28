using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpamerLv3 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject kingBerserker; // Assign your enemy prefab in the Inspector
    private float spawnRate = 1.5f; // Time in seconds between each spawn
    private int maxEnemies = 10; // Maximum number of enemies to spawn
    private int currentEnemies = 0; // Current number of spawned enemies
    public Transform[] waypoints;

    void Start()
    {
        StartCoroutine(SpawnKingEnemyBerserker());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnKingEnemyBerserker()
    {
        while (currentEnemies < maxEnemies)
        {
            yield return new WaitForSeconds(spawnRate);
            GameObject newKing = Instantiate(kingBerserker, transform.position, Quaternion.identity);
            newKing.GetComponent<king_enemy_move>().waypoints = waypoints;
            currentEnemies++;
        }
    }
}
