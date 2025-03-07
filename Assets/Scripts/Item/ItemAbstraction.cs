using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAbstraction : MonoBehaviour
{
    protected float durability = 2.0f; // default durability(độ bền-thời gian cần để phá hủy)
    protected int gold = 2; // default gold(vàng nhận được khi phá hủy)
    // Start is called before the first frame update
    public void StartDestructionTimer()
    {
        Invoke("DestroyItem", durability);
    }
    void DestroyItem()
    {
        GiveGold();
        Destroy(gameObject);
    }

    // Phương thức cộng vàng cho người chơi
    protected void GiveGold()
    {
        GoldManage.Instance.AddGold(gold);
    }
}
