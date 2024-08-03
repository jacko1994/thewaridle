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

        DamageBehavior = new StandardDamage(this, animatorController);
        AttackBehavior = new StandardAttack(this, animatorController);
        MoveBehavior = new StandardMove(this, characterController, animatorController);
    }

    public override bool IsAttackable()
    {
        return EnemyAttackableTags.Contains(tag);
    }

    protected override void PerformActions()
    {
        base.PerformActions();

        GameEntity target = lastAttacker != null && AttackBehavior.CanAttack(lastAttacker) ? lastAttacker : FindNearestEnemy();

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);
            if (distance <= AttackRange)
            {
                Attack(target);
            }
            else
            {
                MoveBehavior?.MoveTowards(target.transform.position);
            }
        }
        else
        {
            SetIdle();
        }
    }

    private void SetIdle()
    {
        animatorController?.SetIdle();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        if (lastAttacker == null || !AttackBehavior.CanAttack(lastAttacker))
        {
            lastAttacker = FindNearestAttacker();
        }
    }

    private GameEntity FindNearestAttacker()
    {
        GameEntity[] entities = FindObjectsOfType<GameEntity>();
        GameEntity nearestAttacker = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameEntity entity in entities)
        {
            if (EnemyAttackableTags.Contains(entity.tag))
            {
                float distance = Vector3.Distance(transform.position, entity.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestAttacker = entity;
                }
            }
        }

        return nearestAttacker;
    }

    protected override GameEntity FindNearestEnemy()
    {
        GameEntity[] entities = FindObjectsOfType<GameEntity>();
        GameEntity nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameEntity entity in entities)
        {
            if (EnemyAttackableTags.Contains(entity.tag))
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
