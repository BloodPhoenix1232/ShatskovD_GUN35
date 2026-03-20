using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI winnerText;
    public Button restartButton;

    private void Start()
    {
        if (BattleController.Instance != null)
        {
            BattleController.Instance.OnGameEnded += OnGameEnded;
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void OnGameEnded(Team winner)
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (winnerText != null)
        {
            winnerText.text = $"{winner} победил!";
        }
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (BattleController.Instance != null)
        {
            BattleController.Instance.OnGameEnded -= OnGameEnded;
        }
    }
}