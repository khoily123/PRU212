using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BulletTower05_level3 : BulletAbstract
{
    public BulletTower05_level3()
    {
        speed = 15f;  // Giá trị mới cho tốc độ
    }
    private float pushBackForce = 1.0f;  // Lực đẩy lùi có thể điều chỉnh

    protected override void Explode()
    {
        base.Explode();  // Gọi phương thức cơ sở để xử lý việc tạo hiệu ứng nổ

        // Thực hiện đẩy lùi mục tiêu
        if (target != null)
        {
            PushBack(target.transform.position - transform.position);
        }
    }

    private void PushBack(Vector3 direction)
    {
        Rigidbody targetRigidbody = target.GetComponent<Rigidbody>();
        if (targetRigidbody != null)
        {
            // Áp dụng lực đẩy lùi với Rigidbody
            targetRigidbody.AddForce(direction.normalized * pushBackForce, ForceMode.Impulse);
        }
        else
        {
            // Đẩy lùi mục tiêu bằng cách thay đổi trực tiếp vị trí nếu không có Rigidbody
            target.transform.position += direction.normalized * pushBackForce;
        }
    }

}
