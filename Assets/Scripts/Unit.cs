using UnityEngine;

public class Unit : GameEntity
{
    protected override void Start()
    {
        base.Start();
        Health = 100;
        AttackPower = 10;
        MovementSpeed = 5f;
        AttackSpeed = 1f;
        AttackRange = 10f;
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
