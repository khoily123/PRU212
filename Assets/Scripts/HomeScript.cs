using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour
{
    public float health = 100f;
    public Slider healthBar;
    public GameObject backgroundDim;  // Nền tối khi chiến thắng
    public Button homeDefeatedButton;
    public GameObject defeatedPanel;

    void Start()
    {
        //homeDefeatedButton.onClick.AddListener(ReturnToLevelSelection1);
        if (backgroundDim != null) backgroundDim.SetActive(false);
        if (defeatedPanel != null) defeatedPanel.SetActive(false);
        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            Destroy(gameObject);  // Hủy nhà chính nếu sức khỏe về 0
            if (defeatedPanel != null) defeatedPanel.SetActive(true);
            if (backgroundDim != null) backgroundDim.SetActive(true);
            Time.timeScale = 0f; // Dừng game
            SceneManager.LoadScene("LevelSelection");
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }

    public void ReturnToLevelSelection1()
    {
        Debug.Log("🔥 Button bị click!");
        Time.timeScale = 1f;
        SceneManager.LoadScene("LevelSelection");
    }
}
