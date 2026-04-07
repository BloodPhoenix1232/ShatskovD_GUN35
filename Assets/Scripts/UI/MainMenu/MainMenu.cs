using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SaveData data = SaveSystem.Instance?.GetData();
        int unlockedIndex = data?.unlockedLevel ?? 3;

        int maxIndex = SceneManager.sceneCountInBuildSettings - 1;

        if (unlockedIndex > maxIndex)
        {
            unlockedIndex = maxIndex;
        }

        SceneManager.LoadScene(unlockedIndex);
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

    public void OpenUpgradeShop()
    {
        SceneManager.LoadScene("UpgradeShop");
    }
}