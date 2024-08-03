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
            target.TakeDamage(gameEntity.AttackPower);
            lastAttackTime = Time.time;
            ConsoleProDebug.Watch("Attack Haha", target.name.ToString());
            //animatorController?.Shoot();
            characterFeedbackManager.PlayAttackFeedback();
        }
    }

    public bool CanAttack(GameEntity target)
    {
        // Lấy tag của mục tiêu
        string targetTag = target?.tag;

        // Kiểm tra xem tag của mục tiêu có nằm trong danh sách AttackableTags không
        bool isTargetTagAttackable = gameEntity.AttackableTags.Contains(targetTag);

        // Kiểm tra xem thời gian chờ giữa các lần tấn công đã hết chưa
        bool isCooldownOver = Time.time - lastAttackTime > 1f / gameEntity.AttackSpeed;

        // Điều kiện cuối cùng để có thể tấn công
        bool canAttack = target != null && isTargetTagAttackable && isCooldownOver;

        // Debug
        ConsoleProDebug.Watch("Target Is Not Null", (target != null).ToString());
        ConsoleProDebug.Watch("Target Tag", targetTag ?? "null");
        ConsoleProDebug.Watch("Is Target Tag Attackable", isTargetTagAttackable.ToString());
        ConsoleProDebug.Watch("Cooldown Over", isCooldownOver.ToString());
        ConsoleProDebug.Watch("Can Attack", canAttack.ToString());

        return canAttack;
    }



}
