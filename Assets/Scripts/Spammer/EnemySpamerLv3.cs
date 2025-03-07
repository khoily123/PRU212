using System.Collections;
using UnityEngine;
using TMPro;

public class EnemySpamerLv3 : MonoBehaviour
{
    public GameObject kingBerserker;
    public Transform[] waypoints;
    public TMP_Text waveText, enemyKillText;

    private float spawnRate = 1.5f;
    private int maxEnemies = 10;
    private int currentEnemies = 0;
    private int maxWaves = 15;
    private int currentWave = 0;
    private static int enemiesKilled = 0;

    void Start()
    {
        UpdateUI();
        StartCoroutine(SpawnWaves());
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
                GameObject newKing = Instantiate(kingBerserker, transform.position, Quaternion.identity);
                newKing.GetComponent<king_enemy>().waypoints = waypoints;
                currentEnemies++;
            }

            Debug.Log($"Wave {currentWave} đã kết thúc! Đợi 5 giây...");
            yield return new WaitForSeconds(1f);
        }

        Debug.Log("Tất cả 15 wave đã hoàn thành!");
    }

    public static void EnemyDefeated()
    {
        enemiesKilled++;
        FindObjectOfType<EnemySpamerLv3>().UpdateUI();
    }

    void UpdateUI()
    {
        if (waveText != null)
            waveText.text = $"{currentWave}/{maxWaves}";

        if (enemyKillText != null)
            enemyKillText.text = $"{enemiesKilled}";
    }

    // 🎯 Hàm này sẽ được gọi khi nhấn "New Game"
    public void ResetGame()
    {
        StopAllCoroutines(); // Dừng tất cả coroutines đang chạy
        currentWave = 0;
        enemiesKilled = 0;
        currentEnemies = 0;

        UpdateUI(); // Cập nhật lại UI

        StartCoroutine(SpawnWaves()); // Bắt đầu lại vòng lặp quái
    }
}
