using UnityEngine;
using System.Collections.Generic;

public class Enemy : GameEntity
{
    public static List<string> EnemyAttackableTags = new List<string> { "Unit", "Base" };
    private GameEntity lastAttacker;

    protected override void Start()
    {
        base.Start();
        Health = 50;
        AttackPower = 5;
        MovementSpeed = 3f;
        AttackSpeed = 0.5f;
        AttackRange = 10f;
    }

    protected override bool IsAttackable()
    {
        return EnemyAttackableTags.Contains(tag);
    }

    protected override void PerformActions()
    {
        base.PerformActions();
        GameEntity target = lastAttacker != null && CanAttack(lastAttacker) ? lastAttacker : FindNearestEnemy();

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= AttackRange)
            {
                Attack(target);
            }
            else
            {
                MoveTowards(target.transform.position);
            }
        }
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (lastAttacker == null || !CanAttack(lastAttacker))
        {
            lastAttacker = FindNearestAttacker();
        }
    }

    private GameEntity FindNearestAttacker()
    {
        GameObject[] attackers = FindObjectsOfType<GameObject>();
        GameEntity nearestAttacker = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in attackers)
        {
            GameEntity entity = obj.GetComponent<GameEntity>();
            if (entity != null && EnemyAttackableTags.Contains(obj.tag))
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestAttacker = entity;
                }
            }
        }

        return nearestAttacker;
    }
}
