using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : GameEntity
{
    private List<string> DefaultEnemyAttackableTags = new List<string> { "Enemy" };

    private int baseHealth = 3;
    private int baseAttackPower = 1;
    private float baseMovementSpeed = 3f;
    private float baseAttackSpeed = 0.7f;
    private float baseAttackRange = 10f;

    protected override void OnEnable()
    {
        ResetToBaseStats();
        base.OnEnable();
    }

    public void ResetToBaseStats()
    {
        Health = baseHealth;
        AttackPower = baseAttackPower;
        MovementSpeed = baseMovementSpeed;
        AttackSpeed = baseAttackSpeed;
        AttackRange = baseAttackRange;
        IsMobile = true;
        AttackableTags = DefaultEnemyAttackableTags;

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = MovementSpeed;
            navMeshAgent.stoppingDistance = AttackRange;
        }
    }

    protected override void PerformActions()
    {
        base.PerformActions();

        GameEntity target = FindNearestEnemy();

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= AttackRange)
            {
                Attack(target);
            }
            else if (IsMobile)
            {
                MoveBehavior?.MoveTowards(target.transform.position);
            }
        }
    }

    protected override GameEntity FindNearestEnemy()
    {
        GameEntity[] entities = FindObjectsOfType<GameEntity>();
        GameEntity nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameEntity entity in entities)
        {
            if (DefaultEnemyAttackableTags.Contains(entity.tag))
            {
                float distance = Vector3.Distance(transform.position, entity.transform.position);

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = entity;
                }
            }
        }

        return nearestEnemy;
    }
}
