public class StandardDamage : IDamageable
{
    private readonly GameEntity gameEntity;
    private readonly AnimatorController animatorController;

    public StandardDamage(GameEntity gameEntity, AnimatorController animatorController)
    {
        this.gameEntity = gameEntity;
        this.animatorController = animatorController;
    }

    public void TakeDamage(int amount)
    {
        gameEntity.ModifyHealth(-amount);
        animatorController?.TakeDamage();
    }
}
