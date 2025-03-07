using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManage : MonoBehaviour
{
    public static GoldManage Instance { get; private set; } // Singleton instance

    public TextMeshProUGUI goldText; // UI để hiển thị vàng
    public TextMeshProUGUI goldT1;
    public TextMeshProUGUI goldT2;
    public TextMeshProUGUI goldT3;
    private int gold = 50; // Số vàng ban đầu

    private void Awake()
    {
        // Đảm bảo chỉ có một instance của GoldManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdateGoldDisplay();
    }

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateGoldDisplay();
    }

    public void SpendGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            UpdateGoldDisplay();
        }
        else
        {
            Debug.Log("Not enough gold!"); // Thông báo khi không đủ vàng
        }
    }

    public bool CanAfford(int amount)
    {
        return gold >= amount;
    }

    public int CurrentGold()
    {
        return gold;
    }

    private void UpdateGoldDisplay()
    {
        goldText.text = "Gold: " + gold;
        UpdateGoldTowerDisplay(10, 20, 50);
    }
    private void UpdateGoldTowerDisplay(int t1, int t2, int t3)
    {
        goldT1.text = "" + t1;
        goldT2.text = "" + t2;
        goldT3.text = "" + t3;
    }
}
