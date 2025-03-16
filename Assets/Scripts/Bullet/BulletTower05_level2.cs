using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletTower05_level2 : BulletAbstract
{
    public BulletTower05_level2()
    {
        speed = 10f;  // Giá trị mới cho tốc độ
    }
    private float pushBackForce = 1.5f;  // Lực đẩy lùi có thể điều chỉnh

    protected override void Explode()
    {
        base.Explode();  // Gọi phương thức cơ sở để xử lý việc tạo hiệu ứng nổ

        if (target != null)
        {
            Vector3 knockbackDirection = (target.transform.position - transform.position).normalized;

            // 🔥 Gọi hàm KnockBack() trên quái với hướng đẩy lùi và lực
            target.KnockBack(knockbackDirection, pushBackForce);
        }
    }

}
