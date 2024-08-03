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
        IsMobile = false;
        lastAttackTime = Time.time;

    }

    protected override bool IsAttackable()
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
                Debug.Log("Unit Attack");
                Attack(nearestEnemy);
            }
            else
            {
                MoveTowards(nearestEnemy.transform.position);
            }
        }
    }
    protected override GameEntity FindNearestEnemy()
    {
        GameObject[] potentialTargets = FindObjectsOfType<GameObject>();
        GameEntity nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in potentialTargets)
        {
            // Chỉ kiểm tra những đối tượng có tag "Enemy"
            if (obj.tag != "Enemy") continue;

            GameEntity entity = obj.GetComponent<GameEntity>();
            if (entity != null)
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = entity;
                }
            }
        }

        return nearestEnemy;
    }
    private float lastAttackTime;

    protected override bool CanAttack(GameEntity target)
    {
        return target != null && Time.time - lastAttackTime > 1f / AttackSpeed;
    }
}
