using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheWarIdleManager : MonoBehaviour
{
    public static TheWarIdleManager Instance { get; private set; }

    public List<StageConfig> stageConfigs;
    public Spawner enemySpawner;
    public Spawner unitSpawner;
    public Base playerBase;
    public int totalMoney = 0;
    private int currentCrowns = 0;
    private int currentStageIndex = 0;
    private int currentEnemyCount = 0; // Đếm số lượng kẻ thù hiện tại
    public Text totalCrownText;
    public Text currentStageText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Giữ lại đối tượng giữa các cảnh
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadCrowns();
        UpdateCrownDisplay();
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

        currentEnemyCount = config.numberOfEnemies;
        Debug.Log($"Initializing Stage {stageIndex + 1} with {currentEnemyCount} enemies.");

        if (currentStageIndex == 0)
        {
            unitSpawner.SpawnObjectManual();
        }

        enemySpawner.maxObjects = config.numberOfEnemies;
        enemySpawner.ResetCurrentCount();
        //enemySpawner.SpawnObjectManual();

        foreach (var entity in FindObjectsOfType<Unit>())
        {
            entity.ResetToBaseStats();
        }

        playerBase.Repair(playerBase.MaxHealth);

        UpdateStageDisplay();
    }

    public void OnEnemyDeath()
    {
        currentEnemyCount--;
        totalMoney += 1;
        Debug.Log("Enemy killed. Money earned: " + totalMoney + ", Current Enemy Count: " + currentEnemyCount);

        if (currentEnemyCount <= 0)
        {
            AwardCrowns(100);
            NextStage();
        }
    }
    void NextStage()
    {
        currentStageIndex++;
        if (currentStageIndex < stageConfigs.Count)
        {
            InitializeStage(currentStageIndex);
        }
        else
        {
            StageConfig lastConfig = stageConfigs[stageConfigs.Count - 1];
            int newNumberOfEnemies = Mathf.CeilToInt(lastConfig.numberOfEnemies * 1.5f);
            int newEnemyHP = Mathf.CeilToInt(lastConfig.enemyHP * 1.5f);
            int newEnemyATK = Mathf.CeilToInt(lastConfig.enemyATK * 1.5f);

            StageConfig newConfig = new StageConfig(newNumberOfEnemies, newEnemyHP, newEnemyATK);
            stageConfigs.Add(newConfig);

            InitializeStage(currentStageIndex);
        }
    }

    void AwardCrowns(int amount)
    {
        currentCrowns += amount;
        Debug.Log("Crowns earned: " + amount + ", Total Crowns: " + currentCrowns);
        SaveCrowns();
        UpdateCrownDisplay();
    }

    void SaveCrowns()
    {
        PlayerPrefs.SetInt("TotalCrowns", currentCrowns);
        PlayerPrefs.Save();
    }

    void LoadCrowns()
    {
        currentCrowns = PlayerPrefs.GetInt("TotalCrowns", 0);
    }

    void UpdateCrownDisplay()
    {
        if (totalCrownText != null)
        {
            totalCrownText.text = "Crowns: " + currentCrowns.ToString();
        }
    }

    void UpdateStageDisplay()
    {
        if (currentStageText != null)
        {
            currentStageText.text = "Stage: " + (currentStageIndex + 1).ToString();
        }
    }
}
