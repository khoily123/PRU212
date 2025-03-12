using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Data.SqlClient;
using System.Collections.Generic;

public class LevelSelectionManager : MonoBehaviour
{
    [Header("Level Selection")]
    public GameObject levelSelectionPopup; // Popup chọn level
    public Button[] levelButtons;
    public Sprite lockedSprite;
    public Sprite unlockedSprite;

    [Header("Difficulty Popup")]
    public GameObject difficultyPopup;
    public Button easyButton;
    public Button mediumButton;
    public Button hardButton;
    public Button closeDifficultyButton; // Nút đóng popup độ khó
    public GameObject easyCheckmark;
    public GameObject mediumCheckmark;
    public GameObject hardCheckmark;
    private int selectedLevel;

    [Header("Leaderboard")]
    public GameObject[] playerPanels;
    public TMP_Text[] playerNames;
    public TMP_Text[] playerScores;

    private string connectionString = "Server=(local);Database=DefenseTower;User Id=sa;Password=123;Encrypt=false;Trusted_Connection=True";
    private int playerId;

    void Start()
    {
        playerId = PlayerPrefs.GetInt("PlayerId");

        SetupLevelButtons();
        LoadLeaderboard();

        // Ẩn popup chọn độ khó khi bắt đầu
        difficultyPopup.SetActive(false);

        // Đăng ký sự kiện cho các nút chọn độ khó
        easyButton.onClick.AddListener(() => LoadLevelWithDifficulty("Easy"));
        mediumButton.onClick.AddListener(() => LoadLevelWithDifficulty("Medium"));
        hardButton.onClick.AddListener(() => LoadLevelWithDifficulty("Hard"));

        // Đăng ký sự kiện cho nút đóng popup độ khó
        closeDifficultyButton.onClick.AddListener(CloseDifficultyPopup);
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
                button.onClick.AddListener(() => OpenDifficultyPopup(level));
            }
            else
            {
                if (levelText != null) levelText.gameObject.SetActive(false);
                if (buttonImage != null) buttonImage.sprite = lockedSprite;
                button.interactable = false;
            }
        }
    }

    void OpenDifficultyPopup(int level)
    {
        selectedLevel = level;

        levelSelectionPopup.SetActive(false);
        difficultyPopup.SetActive(true);
        LoadCompletedDifficulties(level);
    }

    void CloseDifficultyPopup()
    {
        // Ẩn popup chọn độ khó
        difficultyPopup.SetActive(false);

        // Hiển thị lại popup chọn level
        levelSelectionPopup.SetActive(true);
    }

    void LoadLevelWithDifficulty(string difficulty)
    {
        PlayerPrefs.SetString("SelectedDifficulty", difficulty);
        PlayerPrefs.SetInt("SelectedLevel", selectedLevel);
        SceneManager.LoadScene("Level" + selectedLevel);
    }

    void LoadCompletedDifficulties(int level)
    {
        easyCheckmark.SetActive(false);
        mediumCheckmark.SetActive(false);
        hardCheckmark.SetActive(false);

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string query = "SELECT Difficulty FROM PlayerProgress WHERE PlayerId = @PlayerId AND Level = @Level";
            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Level", level);
            command.Parameters.AddWithValue("@PlayerId", playerId);
            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                string difficulty = reader["Difficulty"].ToString();
                if (difficulty == "Easy") easyCheckmark.SetActive(true);
                if (difficulty == "Medium") mediumCheckmark.SetActive(true);
                if (difficulty == "Hard") hardCheckmark.SetActive(true);
            }
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
