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
        IsMobile = true;

        DamageBehavior = new StandardDamage(this, animatorController);
        AttackBehavior = new StandardAttack(this, animatorController);
        MoveBehavior = new StandardMove(this, characterController, animatorController);
    }

    public override bool IsAttackable()
    {
        return UnitAttackableTags.Contains(tag);
    }

    protected override void PerformActions()
    {
        base.PerformActions();

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
                MoveBehavior?.MoveTowards(nearestEnemy.transform.position);
            }
        }
        else
        {
            SetIdle();
        }
    }

    protected override GameEntity FindNearestEnemy()
    {
        GameEntity[] potentialTargets = FindObjectsOfType<GameEntity>();
        GameEntity nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameEntity entity in potentialTargets)
        {
            if (!UnitAttackableTags.Contains(entity.tag)) continue;

            float distance = Vector3.Distance(transform.position, entity.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = entity;
            }
        }

        return nearestEnemy;
    }

    private void SetIdle()
    {
        animatorController?.SetIdle();
    }
}
