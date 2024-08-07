using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private const string HP_KEY = "UnitHP";
    private const string ATK_KEY = "UnitATK";
    private const string ATTACK_SPEED_KEY = "UnitAttackSpeed";
    private const string MOVEMENT_SPEED_KEY = "UnitMovementSpeed";
    private const string ATTACK_RANGE_KEY = "UnitAttackRange";

    public static int UnitHP
    {
        get => PlayerPrefs.GetInt(HP_KEY, 3);
        set => PlayerPrefs.SetInt(HP_KEY, value);
    }

    public static int UnitATK
    {
        get => PlayerPrefs.GetInt(ATK_KEY, 1);
        set => PlayerPrefs.SetInt(ATK_KEY, value);
    }

    public static float UnitAttackSpeed
    {
        get => PlayerPrefs.GetFloat(ATTACK_SPEED_KEY, 0.7f);
        set => PlayerPrefs.SetFloat(ATTACK_SPEED_KEY, value);
    }

    public static float UnitMovementSpeed
    {
        get => PlayerPrefs.GetFloat(MOVEMENT_SPEED_KEY, 3.0f);
        set => PlayerPrefs.SetFloat(MOVEMENT_SPEED_KEY, value);
    }

    public static float UnitAttackRange
    {
        get => PlayerPrefs.GetFloat(ATTACK_RANGE_KEY, 10.0f); 
        set => PlayerPrefs.SetFloat(ATTACK_RANGE_KEY, value);
    }

    public static void SaveUpgrades()
    {
        PlayerPrefs.Save();
    }
}
