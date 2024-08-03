﻿using UnityEngine;
using System.Collections.Generic;

public class Enemy : GameEntity
{
    public static List<string> DefaultEnemyAttackableTags = new List<string> { "Unit", "Base" };


    protected override void Start()
    {
        base.Start();

        Health = 50;
        AttackPower = 5;
        MovementSpeed = 3f;
        AttackSpeed = 0.5f;
        AttackRange = 10f;
        IsMobile = true;
        AttackableTags = DefaultEnemyAttackableTags;
        navMeshAgent.speed = MovementSpeed;
        navMeshAgent.stoppingDistance = AttackRange;

    }


    protected override void PerformActions()
    {
        base.PerformActions();

        // Chỉ tìm và tấn công kẻ thù gần nhất
        GameEntity target = FindNearestEnemy();

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance <= AttackRange)
            {
                ConsoleProDebug.Watch("Enemy attack", distance.ToString());
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
