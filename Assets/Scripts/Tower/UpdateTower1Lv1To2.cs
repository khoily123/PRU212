using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpdateTower1Lv1To2 : AbstractUpdate
{
    public GameObject dialog;
    public Button yesButton;
    public Button noButton;
    public TMP_Text dialogText;

    void Start()
    {
        cost = 50;
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
        Debug.Log("Nâng cấp tháp thành công!");
        dialog.SetActive(false);
    }

    private void OnNoClicked()
    {
        Debug.Log("Da huy nang cap");
        dialog.SetActive(false);
    }

}
