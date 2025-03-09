using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : ItemAbstraction
{
    // Start is called before the first frame update
    void Start()
    {
        durability = 5.0f; // Giả sử độ bền của cây là 5.0 giây
        gold = 5; // Giả sử vàng nhận được khi phá cây là 5
        //StartDestructionTimer(); // Bắt đầu đếm ngược để phá hủy
    }


}
