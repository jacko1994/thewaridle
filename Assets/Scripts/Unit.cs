using UnityEngine;
using System.Collections.Generic;

public class Unit : GameEntity
{
    public static List<string> DefaultUnitAttackableTags = new List<string> { "Enemy" };


    protected override void Start()
    {
        base.Start();
        Health = 100;
        AttackPower = 10;
        MovementSpeed = 5f;
        AttackSpeed = 1f;
        AttackRange = 5f;
        IsMobile = true;
        AttackableTags = DefaultUnitAttackableTags;

    }
    protected override void PerformActions()
    {
        base.PerformActions();

        GameEntity target = FindNearestEnemy();

        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.transform.position);

            // Debug khoảng cách tới mục tiêu
            ConsoleProDebug.Watch("Unit Distance to Target", distance.ToString());

            if (distance <= AttackRange)
            {
                // Debug khi Unit tấn công
                ConsoleProDebug.Watch("Unit Action", "Attacking");
                ConsoleProDebug.Watch("Unit Attack Range", AttackRange.ToString());
                ConsoleProDebug.Watch("Unit Attack Distance", distance.ToString());
                Attack(target);
            }
            else if (IsMobile)
            {
                // Debug khi Unit di chuyển về phía mục tiêu
                ConsoleProDebug.Watch("Unit Action", "Moving towards target");
                MoveBehavior?.MoveTowards(target.transform.position);
            }
        }
        else
        {
            // Debug khi không có mục tiêu nào
            ConsoleProDebug.Watch("Unit Action", "No target found");
        }
    }


    protected override GameEntity FindNearestEnemy()
    {
        GameEntity[] entities = FindObjectsOfType<GameEntity>();
        GameEntity nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameEntity entity in entities)
        {
            if (DefaultUnitAttackableTags.Contains(entity.tag))
            {
                float distance = Vector3.Distance(transform.position, entity.transform.position);

                // Debug thông tin về khoảng cách và kẻ thù hiện tại
                ConsoleProDebug.Watch("Unit Checking Entity", entity.name);
                ConsoleProDebug.Watch("Unit Distance to Entity", distance.ToString());

                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = entity;

                    // Debug kẻ thù gần nhất hiện tại
                    ConsoleProDebug.Watch("Unit Nearest Enemy", nearestEnemy.name);
                    ConsoleProDebug.Watch("Unit Nearest Distance", nearestDistance.ToString());
                }
            }
        }

        // Debug kết quả cuối cùng của kẻ thù gần nhất
        ConsoleProDebug.Watch("Unit Final Nearest Enemy", nearestEnemy != null ? nearestEnemy.name : "None");
        ConsoleProDebug.Watch("Unit Final Nearest Distance", nearestDistance.ToString());

        return nearestEnemy;
    }

}
