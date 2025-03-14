﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyAbstract : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    protected float speed = 2.0f; // default
    private float minDistance = 0.1f; // default
    private Animator animator;
    
    public Slider healthBar;

    protected float baseHealth = 20f; // Giá trị mặc định
    protected int baseGoldDrop = 3; // Giá trị mặc định

    protected float health; // Sẽ được tính lại dựa trên độ khó
    protected int goldDrop; // Sẽ được tính lại dựa trên độ khó
    protected int attackDamage = 2; // default

    void Start()
    {
        ApplyDifficultySettings();

        transform.position = waypoints[0].position;
        animator = GetComponent<Animator>();
        animator.SetBool("gameStart", true);
        animator.SetBool("isDead", false);

        speed *= EnemyManager.currentSpeedMultiplier;

        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }

    void Update()
    {
        Move();
        UpdateHealthBar();
    }

    void ApplyDifficultySettings()
    {
        string difficulty = PlayerPrefs.GetString("SelectedDifficulty", "Easy");
        int level = PlayerPrefs.GetInt("SelectedLevel", 1); // Lấy level hiện tại

        float healthMultiplier = 1.0f;
        float goldMultiplier = 1.0f;

        switch (difficulty)
        {
            case "Easy":
                healthMultiplier = 1.0f;
                goldMultiplier = 1.0f;
                break;
            case "Medium":
                healthMultiplier = 1.5f;
                goldMultiplier = 1.2f;
                break;
            case "Hard":
                healthMultiplier = 2.0f;
                goldMultiplier = 1.5f;
                break;
        }

        // Hệ số nhân theo level (tăng 10% mỗi level)
        float levelMultiplier = 1.0f + (level - 1) * 0.1f;

        health = baseHealth * healthMultiplier * levelMultiplier;
        goldDrop = Mathf.RoundToInt(baseGoldDrop * goldMultiplier * levelMultiplier);
    }

    void Move()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 movementDirection = (targetWaypoint.position - transform.position).normalized;
        float movementStep = speed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);

        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);
        UpdateAnimation(movementDirection);

        if (distance <= minDistance)
        {
            if (currentWaypointIndex < waypoints.Length - 1)
            {
                currentWaypointIndex++;
            }
            else
            {
                // Đã đến waypoint cuối cùng, tấn công nhà chính
                AttackMainHouse();
            }
        }
    }

    void UpdateAnimation(Vector3 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            // Di chuyển ngang
            if (direction.x > 0)
            {
                animator.SetFloat("move", 1.1f); // Di chuyển sang phải
            }
            else
            {
                animator.SetFloat("move", 0.1f); // Di chuyển sang trái
            }
        }
        else
        {
            // Di chuyển dọc
            if (direction.y > 0)
            {
                animator.SetFloat("move", 2.1f); // Di chuyển lên trên
            }
            else
            {
                animator.SetFloat("move", 3.1f); // Di chuyển xuống dưới
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        animator.SetTrigger("takeDame");
        if (healthBar != null)
        {
            healthBar.value = health;
        }
        if (health <= 0)
        {
            Die();
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0, 1.5f, 0));
            healthBar.transform.position = screenPos;
        }
    }

    void Die()
    {
        SpammerAbstract.EnemyDefeated(); // 🔥 Gọi hàm khi quái chết
        animator.SetBool("isDead", true);
        speed = 0; // Ngăn di chuyển khi chết
        StartCoroutine(WaitForDeathAnimation());
        GoldManage.Instance.AddGold(goldDrop);
    }

    IEnumerator WaitForDeathAnimation()
    {
        // Đợi cho đến khi animation "Dead" kết thúc trước khi hủy đối tượng
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    public void ReduceSpeed(float factor, float duration)
    {
        StartCoroutine(SlowDown(factor, duration));
    }

    IEnumerator SlowDown(float factor, float duration)
    {
        float originalSpeed = speed;
        speed *= factor; // Giảm tốc độ xuống theo tỷ lệ

        yield return new WaitForSeconds(duration); // Đợi trong một khoảng thời gian

        speed = originalSpeed; // Khôi phục tốc độ ban đầu
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speed = 2.0f * multiplier; // 2.0f là tốc độ mặc định, nhân với multiplier
    }

    void AttackMainHouse()
    {
        // Ngăn không cho quái di chuyển sau khi tấn công
        speed = 0;
        animator.SetBool("isAttacking", true); // Giả sử có trạng thái tấn công trong Animator
    }

    void PerformAttack()
    {
        HomeScript mainHouse = FindObjectOfType<HomeScript>(); // Tìm nhà chính trong Scene
        if (mainHouse != null)
        {
            mainHouse.TakeDamage(attackDamage); // Gây sát thương
        }
    }

    public void SetWaypoints(Transform[] newWaypoints)
    {
        waypoints = newWaypoints;
        transform.position = waypoints[0].position;
    }
}
