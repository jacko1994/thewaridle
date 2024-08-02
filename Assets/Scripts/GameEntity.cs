using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{
    public int Health { get; protected set; }
    public int AttackPower { get; protected set; }
    public float MovementSpeed { get; protected set; }
    public float AttackSpeed { get; protected set; }
    public float AttackRange { get; protected set; }

    private float lastAttackTime;
    protected AnimatorController animatorController;
    protected CharacterController characterController;

    protected virtual void Start()
    {
        lastAttackTime = Time.time;
        animatorController = GetComponent<AnimatorController>();
        characterController = GetComponent<CharacterController>();

        if (animatorController == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have an AnimatorController component.");
        }

        if (characterController == null)
        {
            Debug.LogWarning($"{gameObject.name} does not have a CharacterController component.");
        }
    }
    void Update()
    {
        GameEntity nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            float distance = Vector3.Distance(transform.position, nearestEnemy.transform.position);
            float minDistance = 1.0f; // Khoảng cách tối thiểu giữa các đối tượng

            if (distance < minDistance)
            {
                Vector3 direction = (transform.position - nearestEnemy.transform.position).normalized;
                Vector3 newPosition = nearestEnemy.transform.position + direction * minDistance;
                characterController.Move(newPosition - transform.position);
            }
        }

        // Phần còn lại của code Update
    }

    public virtual void TakeDamage(int amount)
    {
        Health -= amount;
        animatorController?.TakeDamage();
        if (Health <= 0)
        {
            Die();
        }
    }

    public virtual void Attack(GameEntity target)
    {
        if (CanAttack(target))
        {
            LookAtTarget(target); // Thực thể nhìn về phía đối thủ trước khi tấn công
            target.TakeDamage(AttackPower);
            lastAttackTime = Time.time;
            animatorController?.Shoot();
        }
    }

    protected virtual void Die()
    {
        animatorController?.Die();
        Destroy(gameObject);
    }

    protected abstract bool IsAttackable();

    protected bool CanAttack(GameEntity target)
    {
        return target != null && target.IsAttackable() && Time.time - lastAttackTime > 1f / AttackSpeed;
    }

    protected GameEntity FindNearestEnemy()
    {
        GameObject[] potentialTargets = FindObjectsOfType<GameObject>();
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in potentialTargets)
        {
            GameEntity entity = obj.GetComponent<GameEntity>();
            if (entity != null && CanAttack(entity))
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = obj;
                }
            }
        }

        return nearestEnemy != null ? nearestEnemy.GetComponent<GameEntity>() : null;
    }

    protected void MoveTowards(Vector3 targetPosition)
    {
        if (characterController == null)
            return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float stoppingDistance = 2f; // Khoảng cách mà đối tượng sẽ dừng lại

        LookAtTarget(targetPosition); // Nhìn về phía mục tiêu trong khi di chuyển

        if (distance > stoppingDistance)
        {
            Vector3 move = direction * MovementSpeed * Time.deltaTime;
            characterController.Move(move);
            animatorController?.Move(true);
        }
        else
        {
            animatorController?.Move(false);
        }
    }

    protected void LookAtTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0; // Đảm bảo không thay đổi góc nhìn dọc theo trục Y
        transform.forward = direction;
    }

    protected void LookAtTarget(GameEntity target)
    {
        if (target == null) return;
        LookAtTarget(target.transform.position);
    }
}
