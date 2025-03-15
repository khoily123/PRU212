using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeShooter : ShooterAbstract
{
    public GameObject newTower;
    public HomeShooter()
    {
        range = 4f;
    }
    void Start()
    {
        CircleCollider2D rangeCollider = gameObject.AddComponent<CircleCollider2D>();
        rangeCollider.radius = range;
        rangeCollider.isTrigger = true;
        ShooterAbstract shooter = newTower.GetComponentInChildren<ShooterAbstract>();
        if (shooter != null)
        {
            shooter.SetPlaced(true);
        }
    }
    public override float ChangeDamageBaseOnLevel()
    {
        return damage;
    }
}
