using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tower1lv1 : MonoBehaviour
{
    public float attackRange = 3f;  // Phạm vi tấn công
    public float attackCooldown = 1f; // Thời gian chờ giữa mỗi lần bắn
    public GameObject projectilePrefab; // Prefab của viên đạn
    public Transform firePoint; // Vị trí bắn
    private List<GameObject> enemiesInRange = new List<GameObject>();
    private float lastAttackTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) // Gán tag "Enemy" cho quái
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    GameObject GetNearestEnemy()
    {
        GameObject nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy == null) continue; // Kiểm tra nếu enemy đã bị tiêu diệt

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }
        return nearest;
    }
    void Attack(GameObject target)
    {
        if (projectilePrefab != null && firePoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            projectile.GetComponent<Projectile>().SetTarget(target);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (enemiesInRange.Count > 0)
        {
            GameObject nearestEnemy = GetNearestEnemy();
            if (nearestEnemy != null && Time.time - lastAttackTime >= attackCooldown)
            {
                Attack(nearestEnemy);
                lastAttackTime = Time.time;
            }
        }
    }
}
