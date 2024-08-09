using UnityEngine;
using UnityEngine.AI;

public class StandardMove : IMovable
{
    private readonly GameEntity gameEntity;
    private readonly CharacterController characterController;
    private readonly AnimatorController animatorController;
    private readonly NavMeshAgent navMeshAgent;
    public StandardMove(GameEntity gameEntity, NavMeshAgent navMeshAgent, AnimatorController animatorController)
    {
        this.gameEntity = gameEntity;
        this.navMeshAgent = navMeshAgent;
        this.animatorController = animatorController;
    }


    public void MoveTowards(Vector3 targetPosition)
    {
        if (navMeshAgent == null) return;

        float stoppingDistance = 2f;

        //gameEntity.LookAtTarget(targetPosition);

        if (navMeshAgent.isStopped) return;

        navMeshAgent.SetDestination(targetPosition);


        if (navMeshAgent.remainingDistance > stoppingDistance)
        {
            animatorController?.Move(true);
        }
        else
        {
            animatorController?.Move(false);
        }
    }
}
