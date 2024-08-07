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
        // Lấy giá trị từ UpgradeManager thay cho giá trị base
        Health = UpgradeManager.UnitHP;
        Debug.Log("Reset " + Health + " base health: " + UpgradeManager.UnitHP);

        AttackPower = UpgradeManager.UnitATK;
        MovementSpeed = UpgradeManager.UnitMovementSpeed;
        AttackSpeed = UpgradeManager.UnitAttackSpeed;
        AttackRange = UpgradeManager.UnitAttackRange;
        IsMobile = true;
        AttackableTags = DefaultEnemyAttackableTags;

        if (navMeshAgent != null)
        {
            navMeshAgent.speed = MovementSpeed;
        }
    }

    public void UpgradeHealth(int increment)
    {
        UpgradeManager.UnitHP += increment;
        Health += increment;
    }

    public void UpgradeAttackPower(int increment)
    {
        UpgradeManager.UnitATK += increment;
        AttackPower += increment;
    }

    public void UpgradeMovementSpeed(float increment)
    {
        UpgradeManager.UnitMovementSpeed += increment;
        MovementSpeed += increment;
        if (navMeshAgent != null)
        {
            navMeshAgent.speed = MovementSpeed;
        }
    }

    public void UpgradeAttackSpeed(float increment)
    {
        UpgradeManager.UnitAttackSpeed += increment;
        AttackSpeed += increment;
    }

    public void UpgradeAttackRange(float increment)
    {
        UpgradeManager.UnitAttackRange += increment;
        AttackRange = increment;
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
