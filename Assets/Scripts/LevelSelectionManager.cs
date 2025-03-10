using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Data.SqlClient;
using System.Collections.Generic;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Level Selection")]
    public Button[] levelButtons;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    [Header("Leaderboard")]
    public GameObject[] playerPanels; // 5 Panel hiển thị thông tin người chơi
    public TMP_Text[] playerNames;    // 5 Text hiển thị tên người chơi
    public TMP_Text[] playerScores;   // 5 Text hiển thị điểm số
    public Image[] playerAvatars;     // 5 Avatar người chơi (nếu có)

    private string connectionString = "Server=(local);Database=DefenseTower;User Id=sa;Password=123;Encrypt=false;Trusted_Connection=True";

    void Start()
    {
        SetupLevelButtons();
        LoadLeaderboard();
    }

    void SetupLevelButtons()
    {
        int maxLevel = PlayerPrefs.GetInt("MaxLevel", 1);

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = i + 1;
            Button button = levelButtons[i];
            TMP_Text levelText = button.GetComponentInChildren<TMP_Text>();
            Image buttonImage = button.GetComponent<Image>();

            if (level <= maxLevel)
            {
                if (levelText != null) levelText.text = level.ToString();
                if (buttonImage != null) buttonImage.sprite = unlockedSprite;

                button.interactable = true;
                button.onClick.AddListener(() => LoadLevel(level));
            }
            else
            {
                if (levelText != null) levelText.gameObject.SetActive(false);
                if (buttonImage != null) buttonImage.sprite = lockedSprite;

                button.interactable = false;
            }
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene("Level" + level);
    }

    public static void UnlockNextLevel(int completedLevel)
    {
        int maxLevel = PlayerPrefs.GetInt("MaxLevel", 1);
        if (completedLevel >= maxLevel)
        {
            PlayerPrefs.SetInt("MaxLevel", completedLevel + 1);
            PlayerPrefs.Save();
        }
    }

    void LoadLeaderboard()
    {
        List<PlayerScoreData> topPlayers = GetTopPlayers(5);

        for (int i = 0; i < playerPanels.Length; i++)
        {
            if (i < topPlayers.Count)
            {
                playerPanels[i].SetActive(true);
                playerNames[i].text = topPlayers[i].Username;
                playerScores[i].text = topPlayers[i].Score.ToString();
                // Nếu có avatar, có thể load từ một thư viện hình ảnh hoặc URL
            }
            else
            {
                playerPanels[i].SetActive(false);
            }
        }
    }

    List<PlayerScoreData> GetTopPlayers(int topN)
    {
        List<PlayerScoreData> players = new List<PlayerScoreData>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = $"SELECT TOP {topN} p.Username, ps.Score FROM PlayerScores ps JOIN Players p ON ps.PlayerId = p.Id ORDER BY ps.Score DESC";
            SqlCommand command = new SqlCommand(query, connection);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                players.Add(new PlayerScoreData
                {
                    Username = reader["Username"].ToString(),
                    Score = int.Parse(reader["Score"].ToString())
                });
            }
        }

        return players;
    }
}

public class PlayerScoreData
{
    public string Username;
    public int Score;
}
