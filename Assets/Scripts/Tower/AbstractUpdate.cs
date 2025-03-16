using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbstractUpdate : MonoBehaviour
{
    public GameObject currentTower;
    public GameObject upgradedTower;
    public GameObject dialog;
    public Button yesButton;
    public Button noButton;
    public TMP_Text dialogText;

    protected int cost;

    void Start()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        dialog.SetActive(false);
    }
    public void Show()
    {
        dialog.SetActive(true);
        var currentGold = GoldManage.Instance.CurrentGold();

        if (currentGold < cost)
        {
            dialogText.text = "You need at least " + cost + " to upgrade this tower!";
            yesButton.interactable = false;
        }
        else
        {
            dialogText.text = "Do you want to upgrade the tower for " + cost + " gold?";
            yesButton.interactable = true;
        }
    }
    private void OnYesClicked()
    {
        UpgradeTower();
        dialog.SetActive(false);
    }

    private void OnNoClicked()
    {
        dialog.SetActive(false);
    }


    public void UpgradeTower()
    {
        if (GoldManage.Instance.CanAfford(cost))
        {
            Vector3 position = currentTower.transform.position;
            Destroy(currentTower);
            GameObject newTower = Instantiate(upgradedTower, position, Quaternion.identity);
            ShooterAbstract shooter = newTower.GetComponentInChildren<ShooterAbstract>();
            if (shooter != null)
            {
                shooter.SetPlaced(true);
            }

            GoldManage.Instance.SpendGold(cost);
            Debug.Log("Nâng cấp tháp thành công!");
        }
        else
        {
            Debug.Log("Không đủ vàng để nâng cấp!");
        }
    }
}
