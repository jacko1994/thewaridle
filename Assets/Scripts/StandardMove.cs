using UnityEngine;

public class StandardMove : IMovable
{
    private readonly GameEntity gameEntity;
    private readonly CharacterController characterController;
    private readonly AnimatorController animatorController;

    public StandardMove(GameEntity gameEntity, CharacterController characterController, AnimatorController animatorController)
    {
        this.gameEntity = gameEntity;
        this.characterController = characterController;
        this.animatorController = animatorController;
    }

    public void MoveTowards(Vector3 targetPosition)
    {
        if (characterController == null) return;

        Vector3 direction = (targetPosition - gameEntity.transform.position).normalized;
        float distance = Vector3.Distance(gameEntity.transform.position, targetPosition);
        float stoppingDistance = 2f;

        gameEntity.LookAtTarget(targetPosition);

        if (distance > stoppingDistance)
        {
            Vector3 move = direction * gameEntity.MovementSpeed * Time.deltaTime;
            characterController.Move(move);
            animatorController?.Move(true);
        }
        else
        {
            animatorController?.Move(false);
        }
    }
}
