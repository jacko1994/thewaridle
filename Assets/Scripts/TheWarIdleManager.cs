using System.Collections.Generic;
using TMPro;
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
    private int currentEnemyCount = 0;

    public TextMeshProUGUI totalCrownText;
    public TextMeshProUGUI currentStageText;
    public Text currentMoneyText;

    public Slider enemyProgressSlider;
    public TextMeshProUGUI enemyProgressText;

    private int baseUnitPrice = 2;
    private int currentUnitPrice;
    private float priceModifier = 1;

    private int healthUpgradeCost;
    private int attackPowerUpgradeCost;
    private int movementSpeedUpgradeCost;
    private int attackSpeedUpgradeCost;
    private int attackRangeUpgradeCost;
    private int baseUpgradeCost = 50;
    public Text healthUpgradeCostText;
    public Text attackPowerUpgradeCostText;
    public Text movementSpeedUpgradeCostText;
    public Text attackSpeedUpgradeCostText;
    public Text attackRangeUpgradeCostText;
    [Header("UI")]
    public GameObject resultScreen;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        LoadCrowns();

    }

    public void StartGame()
    {
        currentUnitPrice = baseUnitPrice;
        UpdateCrownDisplay();
        UpdateMoneyDisplay();
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
        enemySpawner.spawnMode = Spawner.SpawnMode.Auto;

        foreach (var entity in FindObjectsOfType<Unit>())
        {
            entity.ResetToBaseStats();
        }

        playerBase.Repair(playerBase.MaxHealth);

        UpdateStageDisplay();
        UpdateEnemyProgressDisplay();
    }

    public void OnEnemyDeath()
    {
        currentEnemyCount--;
        totalMoney += 1;
        Debug.Log("Enemy killed. Money earned: " + totalMoney + ", Current Enemy Count: " + currentEnemyCount);
        UpdateMoneyDisplay();
        UpdateEnemyProgressDisplay();

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
            totalCrownText.text = "" + currentCrowns.ToString();
        }
    }

    void UpdateStageDisplay()
    {
        if (currentStageText != null)
        {
            currentStageText.SetText("Stage: " + (currentStageIndex + 1).ToString());
        }
    }

    void UpdateMoneyDisplay()
    {
        if (currentMoneyText != null)
        {
            currentMoneyText.text = "Money: " + totalMoney.ToString();
        }
    }

    void UpdateEnemyProgressDisplay()
    {
        int enemiesKilled = stageConfigs[currentStageIndex].numberOfEnemies - currentEnemyCount;
        enemyProgressSlider.maxValue = stageConfigs[currentStageIndex].numberOfEnemies;
        enemyProgressSlider.value = enemiesKilled;
        enemyProgressText.text = $"{enemiesKilled}/{stageConfigs[currentStageIndex].numberOfEnemies}";
    }

    public void BuyUnit()
    {
        if (totalMoney >= currentUnitPrice)
        {
            totalMoney -= currentUnitPrice;
            currentUnitPrice = Mathf.CeilToInt(currentUnitPrice * priceModifier);
            Debug.Log("Unit purchased. New unit price: " + currentUnitPrice + ", Money left: " + totalMoney);
            UpdateMoneyDisplay();

            unitSpawner.SpawnObjectManual();
        }
        else
        {
            Debug.Log("Not enough money to buy unit.");
        }
    }

    void InitializeUpgradeCosts()
    {
        healthUpgradeCost = baseUpgradeCost;
        attackPowerUpgradeCost = baseUpgradeCost;
        movementSpeedUpgradeCost = baseUpgradeCost;
        attackSpeedUpgradeCost = baseUpgradeCost;
        attackRangeUpgradeCost = baseUpgradeCost;

        UpdateUpgradeCostTexts();
    }

    void UpdateUpgradeCostTexts()
    {
        healthUpgradeCostText.text = "x" + healthUpgradeCost;
        attackPowerUpgradeCostText.text = "x" + attackPowerUpgradeCost;
        movementSpeedUpgradeCostText.text = "x" + movementSpeedUpgradeCost;
        attackSpeedUpgradeCostText.text = "x" + attackSpeedUpgradeCost;
        attackRangeUpgradeCostText.text = "x" + attackRangeUpgradeCost;
    }

    public void UpgradeHealth()
    {
        if (currentCrowns >= healthUpgradeCost)
        {
            currentCrowns -= healthUpgradeCost;
            healthUpgradeCost = Mathf.CeilToInt(healthUpgradeCost * 1.1f);
            UpdateCrownDisplay();
            UpdateUpgradeCostTexts();

            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.UpgradeHealth(1);
            }
        }
        else
        {
            Debug.Log("Not enough crowns to upgrade health.");
        }
    }

    public void UpgradeAttackPower()
    {
        if (currentCrowns >= attackPowerUpgradeCost)
        {
            currentCrowns -= attackPowerUpgradeCost;
            attackPowerUpgradeCost = Mathf.CeilToInt(attackPowerUpgradeCost * 1.1f);
            UpdateCrownDisplay();
            UpdateUpgradeCostTexts();

            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.UpgradeAttackPower(1);
            }
        }
        else
        {
            Debug.Log("Not enough crowns to upgrade attack power.");
        }
    }

    public void UpgradeMovementSpeed()
    {
        if (currentCrowns >= movementSpeedUpgradeCost)
        {
            currentCrowns -= movementSpeedUpgradeCost;
            movementSpeedUpgradeCost = Mathf.CeilToInt(movementSpeedUpgradeCost * 1.1f);
            UpdateCrownDisplay();
            UpdateUpgradeCostTexts();

            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.UpgradeMovementSpeed(1);
            }
        }
        else
        {
            Debug.Log("Not enough crowns to upgrade movement speed.");
        }
    }

    public void UpgradeAttackSpeed()
    {
        if (currentCrowns >= attackSpeedUpgradeCost)
        {
            currentCrowns -= attackSpeedUpgradeCost;
            attackSpeedUpgradeCost = Mathf.CeilToInt(attackSpeedUpgradeCost * 1.1f);
            UpdateCrownDisplay();
            UpdateUpgradeCostTexts();

            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.UpgradeAttackSpeed(1);
            }
        }
        else
        {
            Debug.Log("Not enough crowns to upgrade attack speed.");
        }
    }

    public void UpgradeAttackRange()
    {
        if (currentCrowns >= attackRangeUpgradeCost)
        {
            currentCrowns -= attackRangeUpgradeCost;
            attackRangeUpgradeCost = Mathf.CeilToInt(attackRangeUpgradeCost * 1.1f);
            UpdateCrownDisplay();
            UpdateUpgradeCostTexts();

            foreach (var unit in FindObjectsOfType<Unit>())
            {
                unit.UpgradeAttackRange(1);
            }
        }
        else
        {
            Debug.Log("Not enough crowns to upgrade attack range.");
        }
    }

    public bool SpendCrowns(int amount)
    {
        if (currentCrowns >= amount)
        {
            currentCrowns -= amount;
            UpdateCrownDisplay(); // Cập nhật lại hiển thị số Crown
            SaveCrowns(); // Lưu lại số Crown mới
            return true;
        }
        else
        {
            Debug.Log("Not enough crowns.");
            return false;
        }
    }
    public int GetCurrentCrowns()
    {
        return currentCrowns;
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        UIPopupManager.Instance.ShowPanel(1);

    }
}
