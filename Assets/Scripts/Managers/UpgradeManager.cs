using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance { get; private set; }

    [Header("Upgrade Levels")]
    public int stoneDamageLevel = 0;
    public int doubleJumpUnlocked = 0;
    public int chargePowerLevel = 0;
    public int moveSpeedLevel = 0;

    [Header("Upgrade Costs")]
    public int[] stoneDamageCost = { 1, 2, 3 };
    public int doubleJumpCost = 2;
    public int[] chargePowerCost = { 1, 2, 3 };
    public int[] moveSpeedCost = { 1, 2, 3 };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadUpgrades();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool BuyUpgrade(string upgradeName)
    {
        int cost = 0;
        int currentLevel = 0;
        int maxLevel = 0;

        switch (upgradeName)
        {
            case "StoneDamage":
                if (stoneDamageLevel >= stoneDamageCost.Length) return false;
                cost = stoneDamageCost[stoneDamageLevel];
                currentLevel = stoneDamageLevel;
                maxLevel = stoneDamageCost.Length;
                break;
            case "DoubleJump":
                if (doubleJumpUnlocked >= 1) return false;
                cost = doubleJumpCost;
                currentLevel = doubleJumpUnlocked;
                maxLevel = 1;
                break;
            case "ChargePower":
                if (chargePowerLevel >= chargePowerCost.Length) return false;
                cost = chargePowerCost[chargePowerLevel];
                currentLevel = chargePowerLevel;
                maxLevel = chargePowerCost.Length;
                break;
            case "MoveSpeed":
                if (moveSpeedLevel >= moveSpeedCost.Length) return false;
                cost = moveSpeedCost[moveSpeedLevel];
                currentLevel = moveSpeedLevel;
                maxLevel = moveSpeedCost.Length;
                break;
        }

        if (DiamondManager.Instance.Diamonds >= cost)
        {
            DiamondManager.Instance.AddDiamonds(-cost);

            switch (upgradeName)
            {
                case "StoneDamage": stoneDamageLevel++; break;
                case "DoubleJump": doubleJumpUnlocked = 1; break;
                case "ChargePower": chargePowerLevel++; break;
                case "MoveSpeed": moveSpeedLevel++; break;
            }

            SaveUpgrades();
            return true;
        }

        return false;
    }

    public int GetStoneDamage()
    {
        return 1 + stoneDamageLevel;
    }

    public float GetChargeMaxPower()
    {
        float basePower = 15f;
        float bonus = chargePowerLevel * 5f;
        return basePower + bonus;
    }

    public float GetMoveSpeedBonus()
    {
        float baseSpeed = 5f;
        float bonus = moveSpeedLevel * 1f;
        return baseSpeed + bonus;
    }

    private void SaveUpgrades()
    {
        SaveSystem.Instance?.SetUpgrades(stoneDamageLevel, doubleJumpUnlocked, chargePowerLevel, moveSpeedLevel);
    }

    private void LoadUpgrades()
    {
        SaveData data = SaveSystem.Instance?.GetData();
        if (data != null)
        {
            stoneDamageLevel = data.stoneDamageLevel;
            doubleJumpUnlocked = data.doubleJumpUnlocked;
            chargePowerLevel = data.chargePowerLevel;
            moveSpeedLevel = data.moveSpeedLevel;
        }
    }
}