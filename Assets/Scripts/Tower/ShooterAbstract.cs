using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShooterAbstract : MonoBehaviour
{
    public GameObject bulletPrefab;  // Prefab đạn
    public Transform firePoint;  // Vị trí bắn
    public float fireRate = 1f;  // Số lần bắn mỗi giây
    public float damage = 10f;  // Sát thương của tower
    protected float range = 2f;  // Phạm vi tấn công
    private bool isPlaced = false;

    private List<EnemyAbstract> enemiesInRange = new List<EnemyAbstract>();
    private float fireCooldown = 0f;
    private Animator animator;
    private EnemyAbstract currentTarget;
    private AudioSource audioSource;  // 🎵 Âm thanh bắn đạn
    public AudioClip shootSound;  // 🔥 Kéo file âm thanh vào đây trong Inspector

    

    void Start()
    {
        CircleCollider2D rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.radius = range;
        rangeCollider.isTrigger = true;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = gameObject.AddComponent<AudioSource>(); // 🎵 Thêm AudioSource vào object
        audioSource.playOnAwake = false; // Không phát khi game bắt đầu
    }

    void Update()
    {
        fireCooldown -= Time.deltaTime;

        if (!isPlaced)
        {
            return;
        }

        if (fireCooldown <= 0f && enemiesInRange.Count > 0)
        {
            currentTarget = enemiesInRange[0]; // Chọn mục tiêu
            ShootEnemy2(); // Bắn
            fireCooldown = 1f / fireRate; // Đặt lại thời gian cooldown
        }
        else if (enemiesInRange.Count == 0)
        {
            animator.SetBool("IsFire", false);
        }
    }

    public void SetPlaced(bool placed)
    {
        isPlaced = placed;
        animator.enabled = placed;
    }

    public void ShootEnemy2()
    {
        if (currentTarget == null) return;

        animator.SetTrigger("Shoot"); // Kích hoạt animation bắn
        ShootEnemy(currentTarget); // Gọi lại hàm gốc với mục tiêu hiện tại
    }

    void ShootEnemy(EnemyAbstract target)
    {
        if (target == null || target.healthBar.value <= 0) return;

        animator.SetBool("IsFire", true);
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        BulletAbstract bulletScript = bullet.GetComponent<BulletAbstract>();
        bulletScript.SetTarget(target, ChangeDamageBaseOnLevel());

        // 🌟 Lấy góc quay của viên đạn và gán cho tower
        float bulletAngle = bullet.transform.rotation.eulerAngles.z;
        transform.rotation = Quaternion.Euler(0, 0, bulletAngle);

        // 🔊 Phát âm thanh khi bắn
        if (shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }

    public abstract float ChangeDamageBaseOnLevel();

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            enemiesInRange.Add(other.GetComponent<EnemyAbstract>());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyAbstract enemy = other.GetComponent<EnemyAbstract>();
            if (enemiesInRange.Contains(enemy))
            {
                enemiesInRange.Remove(enemy);
            }
        }
    }


}
