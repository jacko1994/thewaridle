using System.Collections.Generic;
using UnityEngine;

public class NearestEnemyLocator : IEnemyLocator
{
    public GameEntity FindNearestEnemy(Transform unitTransform, List<string> attackableTags)
    {
        GameObject[] potentialTargets = GameObject.FindObjectsOfType<GameObject>();
        GameEntity nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject obj in potentialTargets)
        {
            if (!attackableTags.Contains(obj.tag)) continue;

            GameEntity entity = obj.GetComponent<GameEntity>();
            if (entity != null)
            {
                float distance = Vector3.Distance(unitTransform.position, obj.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = entity;
                }
            }
        }

        return nearestEnemy;
    }
}
