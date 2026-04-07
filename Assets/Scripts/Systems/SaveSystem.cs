using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int unlockedLevel;
    public int diamonds;
    public int stoneDamageLevel;
    public int doubleJumpUnlocked;
    public int chargePowerLevel;
    public int moveSpeedLevel;
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private string savePath;
    private SaveData _cachedData;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/save.dat";
            LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame()
    {
        if (_cachedData == null) return;

        string json = JsonUtility.ToJson(_cachedData);
        File.WriteAllText(savePath, json);
    }

    private void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            _cachedData = JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            _cachedData = new SaveData();
            _cachedData.unlockedLevel = 3;
            _cachedData.diamonds = 0;
        }
    }

    public SaveData GetData()
    {
        if (_cachedData == null) LoadGame();
        return _cachedData;
    }

    public void SetUnlockedLevel(int value)
    {
        _cachedData.unlockedLevel = value;
        SaveGame();
    }

    public void SetDiamonds(int value)
    {
        _cachedData.diamonds = value;
        SaveGame();
    }

    public void SetUpgrades(int stoneDamage, int doubleJump, int chargePower, int moveSpeed)
    {
        _cachedData.stoneDamageLevel = stoneDamage;
        _cachedData.doubleJumpUnlocked = doubleJump;
        _cachedData.chargePowerLevel = chargePower;
        _cachedData.moveSpeedLevel = moveSpeed;
        SaveGame();
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }

        _cachedData = new SaveData();
        _cachedData.unlockedLevel = 3;
        _cachedData.diamonds = 0;
    }
}