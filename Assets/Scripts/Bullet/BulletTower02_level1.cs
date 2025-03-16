using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletTower02_level1 : BulletAbstract
{
    private float slowAmount = 0.7f; // Mức độ làm chậm
    private float slowDuration = 2f; // Thời gian làm chậm
    protected override void Explode()
    {
        base.Explode(); // Gọi phương thức cơ sở để xử lý hiệu ứng nổ

        if (target != null)
        {
            target.ReduceSpeed(slowAmount, slowDuration); // Áp dụng hiệu ứng làm chậm
        }
    }

}
