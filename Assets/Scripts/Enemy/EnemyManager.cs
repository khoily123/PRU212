using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyManager : MonoBehaviour
{
    public Button speedButton; // Button UI điều chỉnh tốc độ
    public TextMeshProUGUI speedText; // Text hiển thị tốc độ
    private float[] speedLevels = { 1.0f, 1.5f, 2.0f }; // Các mức tốc độ
    private int currentSpeedIndex = 0; // Chỉ số tốc độ hiện tại
    public static float currentSpeedMultiplier = 1.0f; // 🔥 Lưu trạng thái tốc độ chung

    void Start()
    {
        speedButton.onClick.AddListener(ChangeSpeed);
        speedText.gameObject.SetActive(false); // Ẩn text ban đầu
    }

    void ChangeSpeed()
    {
        currentSpeedIndex = (currentSpeedIndex + 1) % speedLevels.Length;
        currentSpeedMultiplier = speedLevels[currentSpeedIndex]; // 🔥 Cập nhật biến toàn cục

        // Cập nhật tốc độ cho tất cả enemy hiện có
        foreach (EnemyAbstract enemy in FindObjectsOfType<EnemyAbstract>())
        {
            enemy.SetSpeedMultiplier(currentSpeedMultiplier);
        }

        // Hiển thị text tốc độ
        ShowSpeedText("Speed: X" + currentSpeedMultiplier);
    }

    void ShowSpeedText(string text)
    {
        speedText.text = text;
        speedText.gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(HideSpeedText());
    }

    IEnumerator HideSpeedText()
    {
        yield return new WaitForSeconds(1f);
        speedText.gameObject.SetActive(false);
    }
}
