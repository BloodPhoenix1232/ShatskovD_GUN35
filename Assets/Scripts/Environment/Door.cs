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
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SaveData data = SaveSystem.Instance?.GetData();
        int currentUnlocked = data?.unlockedLevel ?? 1;

        if (currentIndex > currentUnlocked)
        {
            SaveSystem.Instance?.SetUnlockedLevel(currentIndex);
        }
    }

    private void LoadNextLevel()
    {
        Time.timeScale = 1f;

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