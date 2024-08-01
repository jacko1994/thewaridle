using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{
    public int Health { get; protected set; }
    public int AttackPower { get; protected set; }
    public float MovementSpeed { get; protected set; }
    public float AttackSpeed { get; protected set; }
    public float AttackRange { get; protected set; }

    private float lastAttackTime;

    protected virtual void Start()
    {
        lastAttackTime = Time.time;
    }

    public virtual void TakeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void Attack(GameEntity target)
    {
        if (Time.time - lastAttackTime > 1f / AttackSpeed && target != null)
        {
            target.TakeDamage(AttackPower);
            lastAttackTime = Time.time;
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected GameEntity FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy != null ? nearestEnemy.GetComponent<GameEntity>() : null;
    }

    protected void MoveTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Vector3 move = direction * MovementSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
    }
}
