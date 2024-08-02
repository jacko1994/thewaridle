using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{
    public int Health { get; protected set; }
    public int AttackPower { get; protected set; }
    public float MovementSpeed { get; protected set; }
    public float AttackSpeed { get; protected set; }
    public float AttackRange { get; protected set; }
    public bool IsMobile { get; protected set; } = true;
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

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        float separationRadius = 1.0f;
        Gizmos.DrawWireSphere(transform.position, separationRadius);
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
            LookAtTarget(target);
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

    protected virtual void Update()
    {
        HandleSeparationFromSameTagEntities();
        PerformActions();
    }

    protected abstract bool IsAttackable();

    protected bool CanAttack(GameEntity target)
    {
        return target != null && target.IsAttackable() && Time.time - lastAttackTime > 1f / AttackSpeed;
    }

    protected virtual void HandleSeparationFromSameTagEntities()
    {
        if (!IsMobile) return;

        float separationRadius = 1.0f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, separationRadius);

        Debug.Log($"Checking for collisions within radius: {separationRadius} from position: {transform.position}");

        foreach (var hitCollider in hitColliders)
        {
            GameObject obj = hitCollider.gameObject;
            if (obj == gameObject || obj.tag != gameObject.tag) continue;

            Debug.Log($"Potential collision detected with object: {obj.name}, tag: {obj.tag}");

            float distance = Vector3.Distance(transform.position, obj.transform.position);
            Debug.Log($"Distance to {obj.name}: {distance}");

            if (distance < separationRadius)
            {
                Vector3 direction = (transform.position - obj.transform.position).normalized;
                Vector3 newPosition = transform.position + direction * (separationRadius - distance);
                characterController.Move(newPosition - transform.position);

                Debug.Log($"Moved {gameObject.name} away from {obj.name} by {separationRadius - distance} units.");
            }
        }
    }

    protected GameEntity FindNearestEnemy()
    {
        GameObject[] potentialTargets = FindObjectsOfType<GameObject>();
        GameEntity nearestEnemy = null;
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
                    nearestEnemy = entity;
                }
            }
        }

        return nearestEnemy != null ? nearestEnemy.GetComponent<GameEntity>() : null;
    }

    protected void MoveTowards(Vector3 targetPosition)
    {
        if (characterController == null) return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, targetPosition);
        float stoppingDistance = 2f;

        LookAtTarget(targetPosition);

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
        direction.y = 0;
        transform.forward = direction;
    }

    protected void LookAtTarget(GameEntity target)
    {
        if (target == null) return;
        LookAtTarget(target.transform.position);
    }

    protected virtual void PerformActions()
    {
        // Phương thức ảo để thực hiện các hành động cụ thể, chỉ thực hiện nếu IsMobile là true
        if (!IsMobile) return;

        // Các hành động cụ thể như tấn công, di chuyển, v.v. sẽ được thực hiện trong lớp kế thừa
    }
}
