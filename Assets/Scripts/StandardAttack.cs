using UnityEngine;

public class StandardAttack : IAttackable
{
    private readonly GameEntity gameEntity;
    private readonly AnimatorController animatorController;
    private readonly CharacterFeedbackManager characterFeedbackManager;
    private float lastAttackTime;

    public StandardAttack(GameEntity gameEntity, AnimatorController animatorController, CharacterFeedbackManager characterFeedbackManager)
    {
        this.gameEntity = gameEntity;
        this.animatorController = animatorController;
        lastAttackTime = Time.time;
        this.characterFeedbackManager = characterFeedbackManager;
    }

    public void Attack(GameEntity target)
    {
        if (CanAttack(target))
        {
            gameEntity.LookAtTarget(target.transform.position);

            // Vô hiệu hóa navMeshAgent khi bắt đầu tấn công
            gameEntity.DisableMovement();

            // Gây sát thương cho mục tiêu
            target.TakeDamage(gameEntity.AttackPower);

            lastAttackTime = Time.time;
            characterFeedbackManager.PlayAttackFeedback();

            // Kích hoạt lại navMeshAgent sau một khoảng thời gian (thời gian hồi chiêu)
            float attackCooldown = 1f / gameEntity.AttackSpeed;
            gameEntity.Invoke(nameof(gameEntity.EnableMovement), attackCooldown);
        }
    }

    public bool CanAttack(GameEntity target)
    {
        if (target == null || target.IsDie) return false;

        string targetTag = target.tag;
        bool isTargetTagAttackable = gameEntity.AttackableTags.Contains(targetTag);
        bool isCooldownOver = Time.time - lastAttackTime > 1f / gameEntity.AttackSpeed;

        return isTargetTagAttackable && isCooldownOver;
    }
}
