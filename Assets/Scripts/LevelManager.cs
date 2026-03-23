using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;

    private void Start()
    {
        if (_playerHealth != null)
        {
            _playerHealth.PlayerDied += OnPlayerDied;
        }
    }

    private void OnDestroy()
    {
        if (_playerHealth != null)
        {
            _playerHealth.PlayerDied -= OnPlayerDied;
        }
    }

    private void OnPlayerDied()
    {
        RestartLevel();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}