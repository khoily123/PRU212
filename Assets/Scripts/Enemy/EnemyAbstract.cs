using System.Collections;
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
    protected float health = 20f; // default
    public Slider healthBar;
    protected int goldDrop = 3; // default

    void Start()
    {
        transform.position = waypoints[0].position;
        animator = GetComponent<Animator>();
        animator.SetBool("gameStart", true);
        animator.SetBool("isDead", false);

        speed *= EnemyManager.currentSpeedMultiplier; // 🔥 Gán tốc độ ngay khi spawn

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

    void Move()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];
        Vector3 movementDirection = (targetWaypoint.position - transform.position).normalized;
        float movementStep = speed * Time.deltaTime;
        float distance = Vector3.Distance(transform.position, targetWaypoint.position);

        // Di chuyển đến waypoint hiện tại
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, movementStep);

        // Cập nhật trạng thái animation dựa trên hướng di chuyển
        UpdateAnimation(movementDirection);

        // Kiểm tra xem đã đến waypoint chưa
        if (distance <= minDistance)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                Destroy(gameObject);  // Bắt đầu lại từ waypoint đầu tiên
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
        EnemySpamerLv3.EnemyDefeated(); // 🔥 Gọi hàm khi quái chết
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
}
