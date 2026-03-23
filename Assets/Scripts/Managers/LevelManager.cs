using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private float _deathDelay = 1f;

    private void Start()
    {
        if (_playerHealth == null)
        {
            _playerHealth = FindObjectOfType<PlayerHealth>();
        }

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
        Invoke(nameof(RestartLevel), _deathDelay);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}