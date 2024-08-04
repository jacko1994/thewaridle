using System.Collections.Generic;
using UnityEngine;

public class TheWarIdleManager : MonoBehaviour
{
    public List<StageConfig> stageConfigs;
    public ObjectPool objectPool; 
    private int currentStageIndex = 0;

    void Start()
    {
        InitializeStage(currentStageIndex);
    }

    void InitializeStage(int stageIndex)
    {
        if (stageIndex < 0 || stageIndex >= stageConfigs.Count)
        {
            Debug.LogError("Invalid stage index!");
            return;
        }

        StageConfig config = stageConfigs[stageIndex];

        for (int i = 0; i < config.numberOfEnemies; i++)
        {
            GameObject enemy = objectPool.GetPooledObject();
            if (enemy != null)
            {
                enemy.SetActive(true);
                Character enemyComponent = enemy.GetComponent<Character>();
                if (enemyComponent != null)
                {
                    enemyComponent.Health = config.enemyHP;
                    enemyComponent.AttackPower = config.enemyATK;
                }
            }
        }
    }

    public void NextStage()
    {
        currentStageIndex++;
        if (currentStageIndex < stageConfigs.Count)
        {
            InitializeStage(currentStageIndex);
        }
        else
        {
            // Nhân các giá trị lên 1.5 lần cho stage mới
            StageConfig lastConfig = stageConfigs[stageConfigs.Count - 1];
            int newNumberOfEnemies = Mathf.CeilToInt(lastConfig.numberOfEnemies * 1.5f);
            int newEnemyHP = Mathf.CeilToInt(lastConfig.enemyHP * 1.5f);
            int newEnemyATK = Mathf.CeilToInt(lastConfig.enemyATK * 1.5f);

            StageConfig newConfig = new StageConfig(newNumberOfEnemies, newEnemyHP, newEnemyATK);
            stageConfigs.Add(newConfig);

            InitializeStage(currentStageIndex);
        }
    }
}
