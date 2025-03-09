using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class SpammerAbstract : MonoBehaviour
{
    public TMP_Text waveText, enemyKillText;
    public GameObject enemyPrefab;
    public Transform[] waypoints;

    protected float spawnRate;
    protected int maxEnemies;
    protected int currentEnemies = 0;
    protected int maxWaves;
    protected int currentWave = 0;
    protected static int enemiesKilled = 0;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        UpdateUI();
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator SpawnWaves()
    {
        while (currentWave < maxWaves)
        {
            currentWave++;
            currentEnemies = 0;
            UpdateUI();

            while (currentEnemies < maxEnemies)
            {
                yield return new WaitForSeconds(spawnRate);
                GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
                newEnemy.GetComponent<EnemyAbstract>().waypoints = waypoints;
                currentEnemies++;
            }

            Debug.Log($"Wave {currentWave} đã kết thúc! Đợi...");
            yield return new WaitForSeconds(1f); // Điều chỉnh thời gian chờ ở đây nếu cần
        }

        Debug.Log("Tất cả các wave đã hoàn thành!");
    }

    public static void EnemyDefeated()
    {
        enemiesKilled++;
        FindObjectOfType<SpammerAbstract>().UpdateUI();
    }

    protected void UpdateUI()
    {
        if (waveText != null)
            waveText.text = $"{currentWave}/{maxWaves}";

        if (enemyKillText != null)
            enemyKillText.text = $"{enemiesKilled}";
    }

    public void ResetGame()
    {
        StopAllCoroutines();
        currentWave = 0;
        enemiesKilled = 0;
        currentEnemies = 0;

        UpdateUI();
        StartCoroutine(SpawnWaves());
    }
}
