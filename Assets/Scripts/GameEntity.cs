using UnityEngine;
using System.Collections.Generic;

public abstract class GameEntity : MonoBehaviour
{
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public float MovementSpeed { get; protected set; }
    public float AttackSpeed { get; protected set; }
    public float AttackRange { get; protected set; }
    public bool IsMobile { get; protected set; } = true;

    protected AnimatorController animatorController;
    protected CharacterController characterController;
    protected CharacterFeedbackManager feedbackManager;

    public IDamageable DamageBehavior { get; set; }
    public IAttackable AttackBehavior { get; set; }
    public IMovable MoveBehavior { get; set; }

    // Thuộc tính chứa các tag có thể bị tấn công
    public List<string> AttackableTags { get; protected set; }
    private float separationRadius = 2f;

    protected virtual void Start()
    {
        animatorController = GetComponent<AnimatorController>();
        characterController = GetComponent<CharacterController>();
        feedbackManager = GetComponent<CharacterFeedbackManager>();
        DamageBehavior = new StandardDamage(this, animatorController);
        AttackBehavior = new StandardAttack(this, animatorController, feedbackManager);
        MoveBehavior = new StandardMove(this, characterController, animatorController);

    }

    protected virtual void Update()
    {
        HandleSeparationFromSameTagEntities();
        PerformActions();
    }

    public virtual void TakeDamage(int amount)
    {
        DamageBehavior?.TakeDamage(amount);
    }

    public virtual void Attack(GameEntity target)
    {
        AttackBehavior?.Attack(target);
    }

    public virtual void ModifyHealth(int amount)
    {
        Health = Mathf.Max(Health + amount, 0);
        if (Health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
        Debug.Log($"{gameObject.name} has died.");
    }

    protected virtual void PerformActions()
    {
        if (!IsMobile) return;
    }

    protected virtual void HandleSeparationFromSameTagEntities()
    {
        if (!IsMobile || characterController == null) return;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, separationRadius);

        ConsoleProDebug.Watch("Separation Check", $"Checking entities within radius {separationRadius}");

        foreach (var hitCollider in hitColliders)
        {
            GameObject obj = hitCollider.gameObject;
            if (obj == gameObject || obj.tag != gameObject.tag) continue;

            float distance = Vector3.Distance(transform.position, obj.transform.position);

            ConsoleProDebug.Watch("Detected Entity", obj.name);
            ConsoleProDebug.Watch("Distance to Entity", distance.ToString());

            if (distance < separationRadius)
            {
                Vector3 currentPosition = transform.position;
                Vector3 direction = (transform.position - obj.transform.position).normalized;
                Vector3 newPosition = transform.position + direction * (separationRadius - distance);

                ConsoleProDebug.Watch("Current Position", currentPosition.ToString());

                ConsoleProDebug.Watch("Moving Entity", $"Moving {gameObject.name} away from {obj.name}");
                ConsoleProDebug.Watch("Separation Distance", (separationRadius - distance).ToString());

                characterController.Move(newPosition - transform.position);

                // Kiểm tra lại vị trí sau khi di chuyển
                ConsoleProDebug.Watch("New Position", transform.position.ToString());
            }
        }
    }





    public void LookAtTarget(Vector3 targetPosition)
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

    protected virtual GameEntity FindNearestEnemy()
    {
        GameObject[] potentialTargets = FindObjectsOfType<GameObject>();
        GameEntity nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in potentialTargets)
        {
            GameEntity entity = obj.GetComponent<GameEntity>();
            if (entity != null && AttackBehavior.CanAttack(entity))
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

    protected virtual void OnDrawGizmos()
    {

        Gizmos.color = Color.blue; 
        Gizmos.DrawWireSphere(transform.position, separationRadius);
    }
}
