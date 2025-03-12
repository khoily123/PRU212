using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySpamerLv3 : SpammerAbstract
{
    public Transform[] alternativeWaypoints; // Đường đi thứ 2
    private bool useAlternativePath = false; // Bật tắt đường đi

    void Awake()
    {
        spawnRate = 1.5f;
        maxEnemies = 10;  // 10 quái mỗi lượt
        maxWaves = 5;
    }

    protected override IEnumerator SpawnWaves()
    {
        while (currentWave < maxWaves)
        {
            currentWave++;
            currentEnemies = 0;
            UpdateUI();

            Debug.Log($"🚀 Wave {currentWave}: {(useAlternativePath ? "Đi hướng 2" : "Đi hướng 1")}");

            while (currentEnemies < maxEnemies)
            {
                yield return new WaitForSeconds(spawnRate);
                GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);

                // Chọn hướng đi theo trạng thái useAlternativePath
                Transform[] selectedWaypoints = useAlternativePath ? alternativeWaypoints : waypoints;
                newEnemy.GetComponent<EnemyAbstract>().SetWaypoints(selectedWaypoints);

                currentEnemies++;
            }

            // Sau mỗi wave, đổi hướng đi
            useAlternativePath = !useAlternativePath;

            yield return new WaitForSeconds(1f);
        }
    }
}
