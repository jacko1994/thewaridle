using UnityEngine;
using System.Collections.Generic;

public class Unit : GameEntity
{
    public static List<string> UnitAttackableTags = new List<string> { "Enemy" };

    protected override void Start()
    {
        base.Start();
        Health = 100;
        AttackPower = 10;
        MovementSpeed = 5f;
        AttackSpeed = 1f;
        AttackRange = 10f;
    }

    protected override bool IsAttackable()
    {
        return UnitAttackableTags.Contains(tag);
    }

    void Update()
    {
        GameEntity nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            if (distance <= AttackRange)
            {
                Attack(nearestEnemy);
            }
            else
            {
                MoveTowards(nearestEnemy.transform.position);
            }
        }
    }
}
