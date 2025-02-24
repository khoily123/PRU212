using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goblin_move : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    public float speed = 2.0f;
    private float minDistance = 0.1f;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("gameStart", true);
    }

    // Update is called once per frame
    void Update()
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
}
