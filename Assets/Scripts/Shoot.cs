using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab đạn
    public Transform firePoint;  // Vị trí bắn
    public float fireRate = 1f;  // Số lần bắn mỗi giây
    public float damage = 10f;  // Sát thương của tower
    public float range = 2f;  // Phạm vi tấn công

    private List<king_enemy_move> enemiesInRange = new List<king_enemy_move>();
    private float fireCooldown = 0f;

    void Start()
    {
        CircleCollider2D rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.radius = range;
        rangeCollider.isTrigger = true;
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        // 🔥 Chỉ bắn nếu còn enemy trong danh sách
        if (fireCooldown <= 0f && enemiesInRange.Count > 0)
        {
            ShootEnemy(enemiesInRange[0]);
            fireCooldown = 1f / fireRate;
        }
    }


    void ShootEnemy(king_enemy_move target)
    {
        if (target == null) return;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        bulletScript.SetTarget(target, damage);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GetComponent<king_enemy_move>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            king_enemy_move enemy = other.GetComponent<king_enemy_move>();
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }

}
