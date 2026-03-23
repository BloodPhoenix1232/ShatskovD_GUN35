using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private string _mainMenuSceneName = "MainMenu";

    private bool _isPaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0f;
        _pauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        _pauseCanvas.SetActive(false);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}