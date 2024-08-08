using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using System.Collections;

public abstract class GameEntity : MonoBehaviour
{
    public int Health { get; set; }
    public int AttackPower { get; set; }
    public float MovementSpeed { get; protected set; }
    public float AttackSpeed { get; protected set; }
    public float AttackRange { get; protected set; }
    public bool IsMobile { get; protected set; } = true;
    public bool IsDie { get; private set; } = false; // Biến để kiểm tra trạng thái chết

    protected AnimatorController animatorController;
    protected CharacterController characterController;
    protected CharacterFeedbackManager feedbackManager;
    protected NavMeshAgent navMeshAgent;
    protected HealthBar healthUI;
    protected ObjectPool objectPool;

    public IDamageable DamageBehavior { get; set; }
    public IAttackable AttackBehavior { get; set; }
    public IMovable MoveBehavior { get; set; }

    public List<string> AttackableTags { get; protected set; }

    protected virtual void OnEnable()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = true;
        navMeshAgent.speed = MovementSpeed;
        navMeshAgent.stoppingDistance = AttackRange;
        animatorController = GetComponent<AnimatorController>();
        characterController = GetComponent<CharacterController>();
        feedbackManager = GetComponent<CharacterFeedbackManager>();
        DamageBehavior = new StandardDamage(this, animatorController);
        AttackBehavior = new StandardAttack(this, animatorController, feedbackManager);
        MoveBehavior = new StandardMove(this, navMeshAgent, animatorController);
        healthUI = GetComponentInChildren<HealthBar>();
        if (healthUI != null)
        {
            healthUI.InitializeHealthBar(Health);
        }
        IsDie = false;
    }

    protected virtual void Update()
    {
        if (!IsDie)
        {
            PerformActions();
        }
    }

    public virtual void TakeDamage(int amount)
    {
        if (IsDie) return; // Không nhận sát thương nếu đã chết

        DamageBehavior?.TakeDamage(amount);
    }

    public virtual void Attack(GameEntity target)
    {
        if (IsDie) return; // Không tấn công nếu đã chết

        AttackBehavior?.Attack(target);
    }

    public virtual void ModifyHealth(int amount)
    {
        if (IsDie) return; // Không thay đổi máu nếu đã chết

        int previousHealth = Health;
        Health = Mathf.Max(Health + amount, 0);

        if (Health < previousHealth)
        {
            healthUI?.RemoveHealth(previousHealth - Health);
        }
        else if (Health > previousHealth)
        {
            healthUI?.AddHealth(Health - previousHealth);
        }

        if (Health <= 0 && !IsDie)
        {
            animatorController.Die();
            OnDeath();
        }
    }

    public void SetObjectPool(ObjectPool pool)
    {
        objectPool = pool;
    }

    protected virtual void OnDeath()
    {
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = false;
        }

        IsMobile = false;
        IsDie = true;

        StartCoroutine(DelayedDie(1.0f));
    }

    private IEnumerator DelayedDie(float delay)
    {
        yield return new WaitForSeconds(delay);
        Die();
    }

    protected virtual void Die()
    {
        IsDie = true;

        if (objectPool != null)
        {
            objectPool.ReturnToPool(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Debug.Log($"{gameObject.name} has died and returned to pool.");
    }

    protected virtual void PerformActions()
    {
        if (!IsMobile || IsDie) return; 
    }

    public void LookAtTarget(Vector3 targetPosition)
    {
        if (IsDie) return;

        Vector3 direction = (targetPosition - transform.position).normalized;
        direction.y = 0;
        transform.forward = direction;
    }

    protected void LookAtTarget(GameEntity target)
    {
        if (IsDie || target == null) return;

        LookAtTarget(target.transform.position);
    }

    protected virtual GameEntity FindNearestEnemy()
    {
        if (IsDie) return null;

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

    private void UpdateHealthUI()
    {
        if (healthUI != null)
        {
            healthUI.UpdateHealthBar();
        }
    }
}
