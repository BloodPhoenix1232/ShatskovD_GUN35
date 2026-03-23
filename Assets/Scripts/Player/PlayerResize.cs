using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerResize : MonoBehaviour
{
    [Header("Resize Settings")]
    [SerializeField] private float _smallScale = 0.5f;
    [SerializeField] private float _normalScale = 1f;
    [SerializeField] private float _resizeSpeed = 10f;

    private PlayerControls _playerControls;
    private float _targetScale;
    private bool _isSmall = false;

    public float CurrentScale => transform.localScale.x;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _targetScale = _normalScale;
        transform.localScale = Vector3.one * _normalScale;
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Gameplay.Resize.performed += OnResizePerformed;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
        _playerControls.Gameplay.Resize.performed -= OnResizePerformed;
    }

    private void Update()
    {
        HandleResize();
    }

    private void OnResizePerformed(InputAction.CallbackContext context)
    {
        _isSmall = !_isSmall;
        _targetScale = _isSmall ? _smallScale : _normalScale;
    }

    private void HandleResize()
    {
        Vector3 newScale = Vector3.Lerp(transform.localScale,
            new Vector3(_targetScale, _targetScale, _targetScale),
            _resizeSpeed * Time.deltaTime);
        transform.localScale = newScale;
    }
}