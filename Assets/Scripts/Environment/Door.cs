using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string _nextLevelName;
    [SerializeField] private int _nextLevelIndex = -1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        LoadNextLevel();
    }

    private void LoadNextLevel()
    {
        if (!string.IsNullOrEmpty(_nextLevelName))
        {
            SceneManager.LoadScene(_nextLevelName);
        }
        else if (_nextLevelIndex >= 0)
        {
            SceneManager.LoadScene(_nextLevelIndex);
        }
        else
        {
            int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextIndex);
        }
    }
}