using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BulletAbstract : MonoBehaviour
{
    protected float speed = 10f;
    protected EnemyAbstract target; // Removed 'virtual' modifier
    private float damage;
    private Vector3 direction;
    public GameObject explosionEffect;
    public void SetTarget(EnemyAbstract enemy, float dmg)
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

        if (Vector3.Distance(transform.position, target.transform.position) < 0.01f)
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

    protected virtual void Explode()
    {
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position + new Vector3(0, -1.0f, 0), Quaternion.identity);

            // Kiểm tra xem hiệu ứng có Animator không
            Animator explosionAnimator = explosion.GetComponent<Animator>();
            if (explosionAnimator != null)
            {
                explosionAnimator.speed = 3f; // 🌟 Tăng tốc độ animation gấp 2 lần
            }
            Destroy(explosion, 0.15f);
        }
    }
}
