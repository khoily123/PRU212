using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpamerLV1 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject goblinBerserker; // Assign your enemy prefab in the Inspector
    private float spawnRate = 1.0f; // Time in seconds between each spawn
    private int maxEnemies = 10; // Maximum number of enemies to spawn
    private int currentEnemies = 0; // Current number of spawned enemies
    public Transform[] waypoints;
    void Start()
    {
        StartCoroutine(SpawnGoblinBerserker());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator SpawnGoblinBerserker()
    {
        while (currentEnemies < maxEnemies)
        {
            yield return new WaitForSeconds(spawnRate);
            GameObject newGoblin = Instantiate(goblinBerserker, transform.position, Quaternion.identity);
            newGoblin.GetComponent<goblin_move>().waypoints = waypoints;
            currentEnemies++;
        }
    }

}
