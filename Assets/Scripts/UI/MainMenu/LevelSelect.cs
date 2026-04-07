using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private UnityEngine.UI.Button[] _levelButtons;
    [SerializeField] private int _firstLevelIndex = 3;

    private void Start()
    {
        UpdateLevelButtons();
    }

    private void UpdateLevelButtons()
    {
        SaveData data = SaveSystem.Instance?.GetData();

        int unlockedIndex = data?.unlockedLevel ?? _firstLevelIndex;

        for (int i = 0; i < _levelButtons.Length; i++)
        {
            int sceneIndex = i + _firstLevelIndex;

            _levelButtons[i].interactable = sceneIndex <= unlockedIndex;
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