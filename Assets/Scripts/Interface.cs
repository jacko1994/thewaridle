using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    void TakeDamage(int amount);
}

public interface IAttackable
{
    void Attack(GameEntity target);
    bool CanAttack(GameEntity target);
}

public interface IMovable
{
    void MoveTowards(Vector3 targetPosition);
}
public interface IEnemyLocator
{
    GameEntity FindNearestEnemy(Transform unitTransform, List<string> attackableTags);
}
