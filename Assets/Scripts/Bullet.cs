using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 6f;
    private king_enemy_move target;
    private float damage;
    private Vector3 direction;
    public GameObject explosionEffect;

    public void SetTarget(king_enemy_move enemy, float dmg)
    {
        target = enemy;
        damage = dmg;

        if (target != null)
        {
            UpdateDirection();
        }
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        // Di chuy?n ??n v? phía m?c tiêu
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            Explode();
            target.TakeDamage(damage);
            Destroy(gameObject);
        }
    }

    void UpdateDirection()
    {
        direction = (target.transform.position - transform.position).normalized;

        // 🌟 Xoay đạn theo hướng bay
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90f để mũi tên hướng đúng
    }

    void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, Quaternion.identity);
            Destroy(explosion, 1f); // 🌟 Hủy hiệu ứng sau 1 giây
        }
    }
}
