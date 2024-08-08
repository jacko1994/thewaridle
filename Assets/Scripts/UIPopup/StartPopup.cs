using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UpgradeButton
{
    public Button button;
    public Text upgradeCostText;
    public Text upgradeValueText;
    public Text currentStatText;
}

public class StartPopup : MonoBehaviour
{
    public Button startButton;
    [SerializeField] private List<UpgradeButton> upgradeButtons;
    public TextMeshProUGUI crownText;

    private UpgradeInfo hpUpgradeInfo;
    private UpgradeInfo atkUpgradeInfo;
    private UpgradeInfo attackSpeedUpgradeInfo;
    private UpgradeInfo attackRangeUpgradeInfo;

    void Start()
    {
        hpUpgradeInfo = new UpgradeInfo(UpgradeManager.UnitHP, 50, 1);
        atkUpgradeInfo = new UpgradeInfo(UpgradeManager.UnitATK, 50, 1);
        attackSpeedUpgradeInfo = new UpgradeInfo(UpgradeManager.UnitAttackSpeed, 50, 0.1f);
        attackRangeUpgradeInfo = new UpgradeInfo(UpgradeManager.UnitAttackRange, 50, 0.1f);

        startButton.onClick.AddListener(OnTapToStart);

        upgradeButtons[0].button.onClick.AddListener(() => OnUpgrade(hpUpgradeInfo, 0));
        upgradeButtons[1].button.onClick.AddListener(() => OnUpgrade(atkUpgradeInfo, 1));
        upgradeButtons[2].button.onClick.AddListener(() => OnUpgrade(attackSpeedUpgradeInfo, 2));
        upgradeButtons[3].button.onClick.AddListener(() => OnUpgrade(attackRangeUpgradeInfo, 3));

        UpdateCrownDisplay();
        UpdateUpgradeTexts();
    }

    private void OnTapToStart()
    {
        TheWarIdleManager.Instance.StartGame();
        UIPopupManager.Instance.HidePanelById(2);
        UIPopupManager.Instance.ShowPanel(3);
    }

    private void OnUpgrade(UpgradeInfo upgradeInfo, int index)
    {
        if (TheWarIdleManager.Instance.SpendCrowns(upgradeInfo.UpgradeCost))
        {
            upgradeInfo.ApplyUpgrade();
            UpgradeManager.SaveUpgrades();
            UpdateUpgradeTexts();
            UpdateCrownDisplay();
        }
    }

    private void UpdateCrownDisplay()
    {
        crownText.text = "" + TheWarIdleManager.Instance.GetCurrentCrowns().ToString();
    }

    private void UpdateUpgradeTexts()
    {
        UpdateButtonUI(upgradeButtons[0], hpUpgradeInfo, "");
        UpdateButtonUI(upgradeButtons[1], atkUpgradeInfo, "");
        UpdateButtonUI(upgradeButtons[2], attackSpeedUpgradeInfo, "");
        UpdateButtonUI(upgradeButtons[3], attackRangeUpgradeInfo, "");
    }

    private void UpdateButtonUI(UpgradeButton button, UpgradeInfo upgradeInfo, string statPrefix)
    {
        button.upgradeCostText.text = "x" + upgradeInfo.UpgradeCost;
        button.upgradeValueText.text = upgradeInfo.GetUpgradeText();
        button.currentStatText.text = statPrefix + upgradeInfo.CurrentValue;
    }
}
