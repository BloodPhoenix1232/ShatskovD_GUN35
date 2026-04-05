using TMPro;
using UnityEngine;

public class DiamondsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _diamondsText;

    private void Start()
    {
        if (DiamondManager.Instance != null)
        {
            DiamondManager.Instance.DiamondsChanged += UpdateDisplay;
            UpdateDisplay(DiamondManager.Instance.Diamonds);
        }
    }

    private void OnDestroy()
    {
        if (DiamondManager.Instance != null)
        {
            DiamondManager.Instance.DiamondsChanged -= UpdateDisplay;
        }
    }

    private void UpdateDisplay(int diamonds)
    {
        _diamondsText.text = $"{diamonds}";
    }
}