using UnityEngine;

public class StandardAttack : IAttackable
{
    private readonly GameEntity gameEntity;
    private readonly AnimatorController animatorController;
    private float lastAttackTime;

    public StandardAttack(GameEntity gameEntity, AnimatorController animatorController)
    {
        this.gameEntity = gameEntity;
        this.animatorController = animatorController;
        lastAttackTime = Time.time;
    }

    public void Attack(GameEntity target)
    {
        if (CanAttack(target))
        {
            gameEntity.LookAtTarget(target.transform.position);
            target.TakeDamage(gameEntity.AttackPower);
            lastAttackTime = Time.time;
            animatorController?.Shoot();
        }
    }

    public bool CanAttack(GameEntity target)
    {
        return target != null && target.IsAttackable() && Time.time - lastAttackTime > 1f / gameEntity.AttackSpeed;
    }
}
