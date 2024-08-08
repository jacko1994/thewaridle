using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class Enemy : GameEntity
{
    private List<string> DefaultEnemyAttackableTags = new List<string> { "Unit", "Base" };

    protected override void OnEnable()
    {
        Health = 1;
        AttackPower = 1;
        MovementSpeed = 3f;
        AttackSpeed = 0.7f;
        AttackRange = 10f;
        IsMobile = true;
        AttackableTags = DefaultEnemyAttackableTags;
        base.OnEnable();
    }
    protected override void OnDeath()
    {
        TheWarIdleManager.Instance?.OnEnemyDeath();

        base.OnDeath();
    }
    protected override void PerformActions()
    {
        base.PerformActions();

        if (IsDie) return; // Không thực hiện hành động nếu đã chết

        GameEntity target = FindNearestEnemy();

        if (target != null && !target.IsDie) // Kiểm tra target có null hoặc đã chết
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
            if (!entity.IsDie && DefaultEnemyAttackableTags.Contains(entity.tag)) // Kiểm tra entity có chết hay không
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
