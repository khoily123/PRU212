using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpamerLV4 : MonoBehaviour
{
public GameObject kingRider; // Prefab quái
    public Transform[] waypoints;

    private float spawnRate = 1.5f; // Thời gian giữa mỗi lần spawn
    private int maxEnemies = 10; // Số quái mỗi wave
    private int currentEnemies = 0; // Đếm số quái hiện tại
    private int maxWaves = 15; // Tổng số wave
    private int currentWave = 0; // Đếm wave hiện tại

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWave < maxWaves) // Lặp 15 wave
        {
            currentWave++;
            currentEnemies = 0; // Reset số quái

            while (currentEnemies < maxEnemies) // Sinh quái trong wave hiện tại
            {
                yield return new WaitForSeconds(spawnRate);
                GameObject newKing = Instantiate(kingRider, transform.position, Quaternion.identity);
                newKing.GetComponent<goblinRider>().waypoints = waypoints;
                currentEnemies++;
            }

            Debug.Log($"Wave {currentWave} đã kết thúc! Đợi 5 giây...");
            yield return new WaitForSeconds(1f); // Chờ 30s trước khi bắt đầu wave mới
        }

        Debug.Log("Tất cả 15 wave đã hoàn thành!");
    }
}
