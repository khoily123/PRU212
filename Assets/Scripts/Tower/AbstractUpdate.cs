using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractUpdate : MonoBehaviour
{
    public GameObject currentTower;
    public GameObject upgradedTower;
    protected int cost;

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
