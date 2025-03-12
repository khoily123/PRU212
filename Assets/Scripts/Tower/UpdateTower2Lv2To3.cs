using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateTower2Lv2To3 : MonoBehaviour
{
    public GameObject currentTower; //tháp hiện tại
    public GameObject towerLv3; //tháp cần update
    public int cost = 120; //giá update
    public void UpgradeTower()
    {
        // Điều chỉnh dòng này tùy theo cách bạn quản lý vàng.

        if (GoldManage.Instance.CanAfford(cost))
        {
            Vector3 position = currentTower.transform.position;
            Destroy(currentTower);
            GameObject newTower = Instantiate(towerLv3, position, Quaternion.identity);
            ShooterAbstract shooter = newTower.GetComponentInChildren<ShooterAbstract>();
            if (shooter != null)
            {
                shooter.SetPlaced(true);
            }

            GoldManage.Instance.SpendGold(cost);  // Cập nhật vàng của người chơi.
            //Debug.Log("Nâng cấp tháp thành công!");
        }
        else
        {
            Debug.Log("Không đủ vàng để nâng cấp!");

        }
    }
}
