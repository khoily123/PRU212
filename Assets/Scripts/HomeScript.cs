using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeScript : MonoBehaviour
{
    public float health = 100f;
    public Slider healthBar;

    void Start()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = health;
            healthBar.value = health;
        }
    }
    public void TakeDamage(float damage)
    {
        health -= damage;
        UpdateHealthBar();
        if (health <= 0)
        {
            Destroy(gameObject);  // Hủy nhà chính nếu sức khỏe về 0
        }
    }

    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = health;
        }
    }
}
