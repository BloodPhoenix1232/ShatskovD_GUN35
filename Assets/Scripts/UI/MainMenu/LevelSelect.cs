using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] private string _mainMenuSceneName = "MainMenu";

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
}