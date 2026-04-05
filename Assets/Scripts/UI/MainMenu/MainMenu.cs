using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SaveData data = SaveSystem.Instance?.GetData();
        int unlockedLevel = data?.unlockedLevel ?? 1;

        int firstLevelIndex = unlockedLevel;

        SceneManager.LoadScene(firstLevelIndex + 1);
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void ResetProgress()
    {
        SaveSystem.Instance?.DeleteSave();
    }
}