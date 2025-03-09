using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseMenu;  // Popup menu chính
    public GameObject settingPanel;  // Popup setting
    public Button pauseButton, closeButton, continueButton, newGameButton, settingButton, optionsButton, exitButton;
    public Button volumeUpButton, volumeDownButton, closeSettingButton;

    private bool isPaused = false;
    private AudioSource bgMusic;
    private float currentVolume = 1f;  // Âm lượng mặc định

    void Start()
    {
        // 🔥 Tìm Background Music trong game
        bgMusic = GameObject.FindWithTag("BackgroundMusic")?.GetComponent<AudioSource>();

        // Gán sự kiện click cho các button
        pauseButton.onClick.AddListener(TogglePause);
        closeButton.onClick.AddListener(ResumeGame);
        continueButton.onClick.AddListener(ResumeGame);
        newGameButton.onClick.AddListener(StartNewGame);
        settingButton.onClick.AddListener(OpenSettings);
        optionsButton.onClick.AddListener(OpenOptions);
        exitButton.onClick.AddListener(ExitGame);

        // Button trong Setting Panel
        volumeUpButton.onClick.AddListener(IncreaseVolume);
        volumeDownButton.onClick.AddListener(DecreaseVolume);
        closeSettingButton.onClick.AddListener(CloseSettings);

        // Ẩn menu khi bắt đầu game
        pauseMenu.SetActive(false);
        settingPanel.SetActive(false);
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f; // ⏸ Dừng game hoặc ▶ Tiếp tục
    }

    void ResumeGame()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        settingPanel.SetActive(false);
        Time.timeScale = 1f; // ▶ Tiếp tục game
    }

    void StartNewGame()
    {
        Time.timeScale = 1f; // Reset tốc độ game
        FindObjectOfType<SpammerAbstract>()?.ResetGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    void OpenSettings()
    {
        settingPanel.SetActive(true);
    }

    void CloseSettings()
    {
        settingPanel.SetActive(false);
    }

    void OpenOptions()
    {
        Debug.Log("Mở tùy chọn!");
        // Thêm logic mở popup Options ở đây nếu cần
    }

    void ExitGame()
    {
        Application.Quit();
    }

    // 🔊 Tăng âm lượng
    void IncreaseVolume()
    {
        if (bgMusic != null)
        {
            currentVolume = Mathf.Clamp(currentVolume + 0.1f, 0f, 1f);
            bgMusic.volume = currentVolume;
        }
    }

    // 🔉 Giảm âm lượng
    void DecreaseVolume()
    {
        if (bgMusic != null)
        {
            currentVolume = Mathf.Clamp(currentVolume - 0.1f, 0f, 1f);
            bgMusic.volume = currentVolume;
        }
    }
}
