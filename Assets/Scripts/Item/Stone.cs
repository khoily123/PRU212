using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : ItemAbstraction
{
    // Start is called before the first frame update
    void Start()
    {
        durability = 3.0f; // Giả sử độ bền của đá là 3.0 giây
        gold = 2; // Giả sử vàng nhận được khi phá đá là 2
       // StartDestructionTimer(); // Bắt đầu đếm ngược để phá hủy
    }


}
