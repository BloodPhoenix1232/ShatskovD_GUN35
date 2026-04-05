using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private UnityEngine.UI.Button[] _levelButtons;

    private void Start()
    {
        SaveData data = SaveSystem.Instance?.GetData();
        int unlockedLevel = data?.unlockedLevel ?? 1;

        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        SaveData data = SaveSystem.Instance?.GetData();
        int unlockedLevel = data?.unlockedLevel ?? 1;

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            _levelButtons[i].interactable = i + 1 <= unlockedLevel;
        }
    }

    public void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene(levelIndex);
        Time.timeScale = 1f;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
        Time.timeScale = 1f;
    }

    public void ResetProgress()
    {
        SaveSystem.Instance?.DeleteSave();
        UpdateLevelButtons();
    }
}