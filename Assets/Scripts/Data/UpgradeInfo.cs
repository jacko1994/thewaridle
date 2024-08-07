using UnityEngine;

public class UpgradeInfo
{
    public float CurrentValue { get; private set; }
    public int UpgradeCost { get; private set; }
    public float IncrementValue { get; private set; }

    public UpgradeInfo(float currentValue, int upgradeCost, float incrementValue)
    {
        CurrentValue = currentValue;
        UpgradeCost = upgradeCost;
        IncrementValue = incrementValue;
    }

    public void ApplyUpgrade()
    {
        CurrentValue += IncrementValue;
        UpgradeCost = Mathf.CeilToInt(UpgradeCost * 1.2f);
    }

    public string GetUpgradeText()
    {
        return "+" + IncrementValue.ToString();
    }

    public string GetCurrentValueText()
    {
        return CurrentValue.ToString();
    }
}
