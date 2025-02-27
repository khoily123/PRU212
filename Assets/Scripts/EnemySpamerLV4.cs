using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpamerLV4 : MonoBehaviour
{
    public GameObject goblinRider; // Assign your enemy prefab in the Inspector
    private float spawnRate = 1.0f; // Time in seconds between each spawn
    private int maxEnemies = 8; // Maximum number of enemies to spawn
    private int currentEnemies = 0; // Current number of spawned enemies
    public Transform[] waypoints;
    void Start()
    {
        StartCoroutine(SpawnGoblinRider());
    }

    // Update is called once per frame
    void Update()
    {

    }
    private IEnumerator SpawnGoblinRider()
    {
        while (currentEnemies < maxEnemies)
        {
            yield return new WaitForSeconds(spawnRate);
            GameObject newGoblin = Instantiate(goblinRider, transform.position, Quaternion.identity);
            newGoblin.GetComponent<goblinRider>().waypoints = waypoints;
            currentEnemies++;
        }
    }
}
