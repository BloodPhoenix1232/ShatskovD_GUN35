using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpgradeShopUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI _diamondsText;

    [Header("Upgrade Buttons")]
    [SerializeField] private UnityEngine.UI.Button _stoneDamageBtn;
    [SerializeField] private TextMeshProUGUI _stoneDamageText;
    [SerializeField] private int[] _stoneDamageCost = { 1, 2, 3 };

    [SerializeField] private UnityEngine.UI.Button _doubleJumpBtn;
    [SerializeField] private TextMeshProUGUI _doubleJumpText;
    [SerializeField] private int _doubleJumpCost = 2;

    [SerializeField] private UnityEngine.UI.Button _chargePowerBtn;
    [SerializeField] private TextMeshProUGUI _chargePowerText;
    [SerializeField] private int[] _chargePowerCost = { 1, 2, 3 };

    [SerializeField] private UnityEngine.UI.Button _moveSpeedBtn;
    [SerializeField] private TextMeshProUGUI _moveSpeedText;
    [SerializeField] private int[] _moveSpeedCost = { 1, 2, 3 };

    [Header("Navigation")]
    [SerializeField] private UnityEngine.UI.Button _backButton;
    [SerializeField] private string _mainMenuSceneName = "MainMenu";

    private void Start()
    {
        UpdateUI();

        _stoneDamageBtn.onClick.AddListener(() => BuyUpgrade("StoneDamage"));
        _doubleJumpBtn.onClick.AddListener(() => BuyUpgrade("DoubleJump"));
        _chargePowerBtn.onClick.AddListener(() => BuyUpgrade("ChargePower"));
        _moveSpeedBtn.onClick.AddListener(() => BuyUpgrade("MoveSpeed"));
        _backButton.onClick.AddListener(BackToMenu);
    }

    private void UpdateUI()
    {
        int diamonds = DiamondManager.Instance?.Diamonds ?? 0;
        _diamondsText.text = $"{diamonds}";

        if (UpgradeManager.Instance != null)
        {
            int stoneLevel = UpgradeManager.Instance.stoneDamageLevel;
            int stoneCost = stoneLevel < _stoneDamageCost.Length ? _stoneDamageCost[stoneLevel] : 0;
            bool stoneMaxed = stoneLevel >= _stoneDamageCost.Length;
            _stoneDamageText.text = stoneMaxed ? "УРОН КАМНЯ (MAX)" : $"УРОН КАМНЯ +1 - {stoneCost}D";
            _stoneDamageBtn.interactable = !stoneMaxed && diamonds >= stoneCost;

            bool doubleJumpUnlocked = UpgradeManager.Instance.doubleJumpUnlocked == 1;
            _doubleJumpText.text = doubleJumpUnlocked ? "ДВОЙНОЙ ПРЫЖОК (MAX)" : $"ДВОЙНОЙ ПРЫЖОК - {_doubleJumpCost}D";
            _doubleJumpBtn.interactable = !doubleJumpUnlocked && diamonds >= _doubleJumpCost;

            int chargeLevel = UpgradeManager.Instance.chargePowerLevel;
            int chargeCost = chargeLevel < _chargePowerCost.Length ? _chargePowerCost[chargeLevel] : 0;
            bool chargeMaxed = chargeLevel >= _chargePowerCost.Length;
            _chargePowerText.text = chargeMaxed ? "СИЛА ЗАРЯДА (MAX)" : $"СИЛА ЗАРЯДА +5 - {chargeCost}D";
            _chargePowerBtn.interactable = !chargeMaxed && diamonds >= chargeCost;

            int speedLevel = UpgradeManager.Instance.moveSpeedLevel;
            int speedCost = speedLevel < _moveSpeedCost.Length ? _moveSpeedCost[speedLevel] : 0;
            bool speedMaxed = speedLevel >= _moveSpeedCost.Length;
            _moveSpeedText.text = speedMaxed ? "СКОРОСТЬ (MAX)" : $"СКОРОСТЬ +1 - {speedCost}D";
            _moveSpeedBtn.interactable = !speedMaxed && diamonds >= speedCost;
        }
    }

    private void BuyUpgrade(string upgradeName)
    {
        if (UpgradeManager.Instance != null)
        {
            bool success = UpgradeManager.Instance.BuyUpgrade(upgradeName);
            if (success)
            {
                UpdateUI();
            }
            else
            {
                Debug.Log("Не хватает алмазов или прокачка максимальна");
            }
        }
    }

    private void BackToMenu()
    {
        SceneManager.LoadScene(_mainMenuSceneName);
    }
}