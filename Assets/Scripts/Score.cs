using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    public static Score Instance;

    [SerializeField] private TMP_Text _scoreText;
    private int score;

    void Awake()
    {
        Instance = this;
        UpdateText();
    }

    public void PinDown(Pin pin)
    {
        score += 1;
        UpdateText();
    }

    private void UpdateText()
    {
        _scoreText.text = "Очки: " + score;
    }
}
