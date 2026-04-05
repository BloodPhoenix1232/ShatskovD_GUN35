using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChargeAttack : MonoBehaviour
{
    [Header("Charge Settings")]
    [SerializeField] private GameObject _stonePrefab;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _minChargeTime = 0.5f;
    [SerializeField] private float _maxChargeTime = 2f;
    [SerializeField] private float _minPower = 5f;
    [SerializeField] private float _maxPower = 15f;

    [Header("Visual")]
    [SerializeField] private GameObject _chargePanel;
    [SerializeField] private Image _chargeFill;
    [SerializeField] private Trajectory _trajectoryRenderer;

    private PlayerControls _playerControls;
    private bool _isCharging;
    private float _chargeStartTime;
    private float _currentCharge;
    private bool _canAttack = true;

    private void Awake()
    {
        _playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Gameplay.Attack.performed += OnAttackPerformed;
        _playerControls.Gameplay.Attack.canceled += OnAttackCanceled;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
        _playerControls.Gameplay.Attack.performed -= OnAttackPerformed;
        _playerControls.Gameplay.Attack.canceled -= OnAttackCanceled;
    }

    private void Start()
    {
        if (_chargePanel != null)
            _chargePanel.SetActive(false);

        if (_trajectoryRenderer != null)
        {
            _trajectoryRenderer.SetChargeParameters(_minPower, _maxPower, _minChargeTime, _maxChargeTime);
        }
    }

    private void Update()
    {
        if (_isCharging)
        {
            _currentCharge = Mathf.Clamp01((Time.time - _chargeStartTime) / _maxChargeTime);
            UpdateChargeIndicator();
        }
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (!_canAttack) return;

        _isCharging = true;
        _chargeStartTime = Time.time;

        if (_chargePanel != null)
            _chargePanel.SetActive(true);

        if (_trajectoryRenderer != null)
            _trajectoryRenderer.StartCharge(_chargeStartTime);
    }

    private void OnAttackCanceled(InputAction.CallbackContext context)
    {
        if (!_isCharging) return;

        _isCharging = false;

        float chargeTime = Time.time - _chargeStartTime;

        if (chargeTime >= _minChargeTime)
        {
            float power = Mathf.Lerp(_minPower, _maxPower, _currentCharge);
            ShootStone(power);
        }

        if (_chargePanel != null)
            _chargePanel.SetActive(false);

        if (_chargeFill != null)
            _chargeFill.fillAmount = 0;

        if (_trajectoryRenderer != null)
            _trajectoryRenderer.EndCharge();
    }

    private void ShootStone(float power)
    {
        if (_stonePrefab == null || _shootPoint == null) return;

        GameObject stone = Instantiate(_stonePrefab, _shootPoint.position, Quaternion.identity);

        Rigidbody2D rb = stone.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
            float direction = playerSprite != null && playerSprite.flipX ? -1f : 1f;

            rb.velocity = new Vector2(direction * power, power * 0.5f);
        }

        Destroy(stone, 3f);
    }

    private void UpdateChargeIndicator()
    {
        if (_chargeFill != null)
        {
            _chargeFill.fillAmount = _currentCharge;
        }
    }

    public void SetCanAttack(bool can)
    {
        _canAttack = can;
    }
}