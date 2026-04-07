using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [SerializeField] private string _nextLevelName;
    [SerializeField] private int _nextLevelIndex = -1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerController>(out _))
        {
            UnlockNextLevel();
            LoadNextLevel();
        }
    }

    private void UnlockNextLevel()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;

        SaveData data = SaveSystem.Instance?.GetData();
        int currentUnlocked = data?.unlockedLevel ?? nextIndex;

        if (nextIndex > currentUnlocked)
        {
            SaveSystem.Instance?.SetUnlockedLevel(nextIndex);
        }
    }

    private void LoadNextLevel()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(_nextLevelName))
        {
            SceneManager.LoadScene(_nextLevelName);
            return;
        }

        if (_nextLevelIndex >= 0)
        {
            SceneManager.LoadScene(_nextLevelIndex);
            return;
        }

        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + 1;

        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextIndex = currentIndex;
        }

        SceneManager.LoadScene(nextIndex);
    }
}