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

    protected int maxBounces = 0; // Số lần nảy tối đa
    protected int currentBounces = 0; // Số lần nảy đã thực hiện

    public void SetTarget(EnemyAbstract enemy, float dmg)
    {
        target = enemy;
        damage = dmg;

        if (target != null)
        {
            UpdateDirection();
        }
    }

    public void SetBouncingTarget(EnemyAbstract enemy, float dmg, int bounces)
    {
        target = enemy;
        damage = dmg;
        maxBounces = bounces;
        currentBounces = 0;

        if (target != null)
        {
            UpdateDirection();
        }
    }

    void Update()
    {
        if (target == null || target.healthBar.value <= 0)
        {
            Destroy(gameObject);
            return;
        }

        // Di chuy?n ??n v? phía m?c tiêu
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position,
            speed * EnemyManager.currentSpeedMultiplier * Time.deltaTime);
        if (Vector3.Distance(transform.position, target.transform.position) < 0.01f)
        {
            Explode();
            target.TakeDamage(damage);
            if (currentBounces < maxBounces)
            {
                Bounce(); // Gọi hàm nảy
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void UpdateDirection()
    {
        direction = (target.transform.position - transform.position).normalized;

        // 🌟 Xoay đạn theo hướng bay
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90f để mũi tên hướng đúng
    }

    protected virtual void Bounce()
    {
        currentBounces++;

        // Tìm mục tiêu mới gần nhất
        EnemyAbstract newTarget = FindNewTarget();
        if (newTarget != null)
        {
            target = newTarget;
            UpdateDirection();
        }
        else
        {
            Destroy(gameObject); // Nếu không có mục tiêu mới thì hủy đạn
        }
    }

    private EnemyAbstract FindNewTarget()
    {
        EnemyAbstract[] enemies = FindObjectsOfType<EnemyAbstract>();
        EnemyAbstract bestTarget = null;
        float minDistance = float.MaxValue;

        foreach (EnemyAbstract enemy in enemies)
        {
            if (enemy != target)
            {
                float dist = Vector3.Distance(transform.position, enemy.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    bestTarget = enemy;
                }
            }
        }

        return bestTarget;
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
