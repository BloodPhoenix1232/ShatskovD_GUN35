using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseCanvas;
    [SerializeField] private string _mainMenuSceneName = "MainMenu";
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;

    private bool _isPaused = false;

    private void Awake()
    {
        if (_pauseCanvas == null)
        {
            _pauseCanvas = GameObject.Find("PauseCanvas");
        }

        if (_pauseCanvas != null)
        {
            _pauseCanvas.SetActive(false);
        }

        if (_musicSlider != null)
        {
            _musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            _musicSlider.value = AudioManager.Instance?.GetMusicVolume() ?? 0.5f;
        }

        if (_sfxSlider != null)
        {
            _sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            _sfxSlider.value = AudioManager.Instance?.GetSFXVolume() ?? 0.7f;
        }
    }

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
        if (_pauseCanvas != null) _pauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1f;
        if (_pauseCanvas != null) _pauseCanvas.SetActive(false);
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

    private void OnMusicVolumeChanged(float value)
    {
        AudioManager.Instance?.SetMusicVolume(value);
    }

    private void OnSFXVolumeChanged(float value)
    {
        AudioManager.Instance?.SetSFXVolume(value);
    }
}