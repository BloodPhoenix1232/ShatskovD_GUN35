using UnityEngine;
using System.IO;

[System.Serializable]
public class SaveData
{
    public int totalCoins;
    public int unlockedLevel;
}

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem Instance { get; private set; }

    private string savePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/save.dat";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveGame(int totalCoins, int unlockedLevel)
    {
        SaveData data = new SaveData();
        data.totalCoins = totalCoins;
        data.unlockedLevel = unlockedLevel;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(savePath, json);
    }

    public SaveData LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            return data;
        }

        return null;
    }

    public bool HasSave()
    {
        return File.Exists(savePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
    }
}