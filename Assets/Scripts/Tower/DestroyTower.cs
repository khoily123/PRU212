using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DestroyTower : MonoBehaviour
{
    public GameObject dialog;  // Assign this from inspector
    public Button yesButton;
    public Button noButton;
    public TMP_Text dialogText;  // Assuming you are using TextMeshPro
    public GameObject towerGameObject;  // Assign this from inspector
    // Start is called before the first frame update
    void Start()
    {
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);
        dialog.SetActive(false);  // Start with the dialog hidden
    }
    public void Show()
    {
        
        dialog.SetActive(true);
    }

    private void OnYesClicked()
    {
        // Logic to delete the tower
        Debug.Log("Tower will be deleted.");
        // Assuming the tower or object to delete is known
        Destroy(towerGameObject);
        GoldManage.Instance.AddGold(5);  // Add gold to the player
        dialog.SetActive(false);
    }

    private void OnNoClicked()
    {
        dialog.SetActive(false);
    }
}
