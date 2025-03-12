using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Data.SqlClient;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public abstract class SpammerAbstract : MonoBehaviour
{
    private string connectionString = "Server=(local);Database=DefenseTower;User Id=sa;Password=123;Encrypt=false;Trusted_Connection=True";

    public TMP_Text waveText, enemyKillText;
    public GameObject enemyPrefab;
    public Transform[] waypoints;

    protected float spawnRate;
    protected int maxEnemies;
    protected int currentEnemies = 0;
    protected int maxWaves;
    protected int currentWave = 0;
    protected static int enemiesKilled;

    public int levelIndex; // Màn chơi hiện tại
    public string difficulty; // Độ khó hiện tại: "Easy", "Medium", "Hard"
    public int playerId; // ID người chơi (lấy từ hệ thống đăng nhập)

    public GameObject victoryPanel;
    public GameObject defeatedPanel;
    public GameObject backgroundDim;  // Nền tối khi chiến thắng
    public Button homeVictoryButton;
    public Button homeDefeatedButton;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemiesKilled = 0;

        playerId = PlayerPrefs.GetInt("PlayerId"); // Lấy ID user từ đăng nhập
        levelIndex = PlayerPrefs.GetInt("SelectedLevel");
        difficulty = PlayerPrefs.GetString("SelectedDifficulty");
        homeVictoryButton.onClick.AddListener(ReturnToLevelSelection);
        if (backgroundDim != null) backgroundDim.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);
        UpdateUI();
        StartCoroutine(SpawnWaves());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual IEnumerator SpawnWaves()
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
        SpammerAbstract spammer = FindObjectOfType<SpammerAbstract>();
        spammer.UpdateUI();
        spammer.CheckGameCompletion(); // Kiểm tra khi tiêu diệt xong tất cả quái
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

    private void CheckGameCompletion()
    {
        if (enemiesKilled == maxEnemies * maxWaves)
        {
            StopAllCoroutines();
            CompleteLevel(playerId, levelIndex, difficulty);

            StartCoroutine(ShowVictoryWithDelay());
            Debug.Log($"Bạn đã hoàn thành Level {levelIndex} với độ khó {difficulty}!");
        }
    }

    private IEnumerator ShowVictoryWithDelay()
    {
        yield return new WaitForSeconds(2f); // Chờ 1 giây
        if (victoryPanel != null) victoryPanel.SetActive(true);
        if (backgroundDim != null) backgroundDim.SetActive(true);
        Time.timeScale = 0f; // Dừng game
    }

    private void CompleteLevel(int playerId, int level, string difficulty)
    {
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();

            string checkQuery = "SELECT COUNT(*) FROM PlayerProgress WHERE PlayerId = @PlayerId AND Level = @Level AND Difficulty = @Difficulty";
            SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
            checkCommand.Parameters.AddWithValue("@PlayerId", playerId);
            checkCommand.Parameters.AddWithValue("@Level", level);
            checkCommand.Parameters.AddWithValue("@Difficulty", difficulty);

            int count = (int)checkCommand.ExecuteScalar();

            if (count == 0)
            {
                int score = level * GetDifficultyMultiplier(difficulty);

                string insertProgressQuery = "INSERT INTO PlayerProgress (PlayerId, Level, Difficulty) VALUES (@PlayerId, @Level, @Difficulty)";
                SqlCommand insertProgressCommand = new SqlCommand(insertProgressQuery, connection);
                insertProgressCommand.Parameters.AddWithValue("@PlayerId", playerId);
                insertProgressCommand.Parameters.AddWithValue("@Level", level);
                insertProgressCommand.Parameters.AddWithValue("@Difficulty", difficulty);
                insertProgressCommand.ExecuteNonQuery();

                string updateScoreQuery = "UPDATE PlayerScores SET Score = Score + @Score WHERE PlayerId = @PlayerId";
                SqlCommand updateScoreCommand = new SqlCommand(updateScoreQuery, connection);
                updateScoreCommand.Parameters.AddWithValue("@Score", score);
                updateScoreCommand.Parameters.AddWithValue("@PlayerId", playerId);
                updateScoreCommand.ExecuteNonQuery();

                Debug.Log($"🎉 Player {playerId} hoàn thành Level {level} ({difficulty}) - Nhận {score} điểm!");

                // Kiểm tra xem đã hoàn thành cả 3 mức độ ở level này chưa
                if (CheckAllDifficultiesCompleted(playerId, level, connection))
                {
                    UnlockNextLevel(playerId, level + 1, connection);
                    PlayerPrefs.SetInt("MaxLevel", level + 1);
                }
            }
            else
            {
                Debug.Log($"⚠️ Player {playerId} đã hoàn thành Level {level} ({difficulty}) trước đó. Không cộng điểm!");
            }
        }
    }

    private bool CheckAllDifficultiesCompleted(int playerId, int level, SqlConnection connection)
    {
        string query = "SELECT COUNT(DISTINCT Difficulty) FROM PlayerProgress WHERE PlayerId = @PlayerId AND Level = @Level";
        SqlCommand command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@PlayerId", playerId);
        command.Parameters.AddWithValue("@Level", level);

        int difficultyCount = (int)command.ExecuteScalar();
        return difficultyCount == 3; // Nếu đã hoàn thành cả 3 mức độ
    }

    private void UnlockNextLevel(int playerId, int nextLevel, SqlConnection connection)
    {
        string getMaxLevelQuery = "SELECT MaxLevel FROM Players WHERE Id = @PlayerId";
        SqlCommand getMaxLevelCommand = new SqlCommand(getMaxLevelQuery, connection);
        getMaxLevelCommand.Parameters.AddWithValue("@PlayerId", playerId);

        int maxLevel = (int)getMaxLevelCommand.ExecuteScalar();

        if (maxLevel < nextLevel)
        {
            string updateMaxLevelQuery = "UPDATE Players SET MaxLevel = @NextLevel WHERE Id = @PlayerId";
            SqlCommand updateMaxLevelCommand = new SqlCommand(updateMaxLevelQuery, connection);
            updateMaxLevelCommand.Parameters.AddWithValue("@NextLevel", nextLevel);
            updateMaxLevelCommand.Parameters.AddWithValue("@PlayerId", playerId);
            updateMaxLevelCommand.ExecuteNonQuery();

            Debug.Log($"🔓 Player {playerId} đã mở khóa Level {nextLevel}!");
        }
    }

    private int GetDifficultyMultiplier(string difficulty)
    {
        switch (difficulty)
        {
            case "Easy": return 1;
            case "Medium": return 2;
            case "Hard": return 3;
            default: return 1;
        }
    }

    private void ReturnToLevelSelection()
    {
        Time.timeScale = 1f; // Khôi phục tốc độ game
        SceneManager.LoadScene("LevelSelection"); // Quay về màn chọn level
    }
}
