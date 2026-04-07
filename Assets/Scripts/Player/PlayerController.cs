using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _baseMoveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float _baseJumpForce = 10f;
    [SerializeField] private float _minJumpMultiplier = 0.5f;
    [SerializeField] private float _maxJumpMultiplier = 1.5f;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private float _moveSpeed;
    private float _moveInput;
    private Rigidbody2D _rb;
    private PlayerControls _playerControls;
    private bool _isGrounded;
    private Collider2D _collider;
    private PlayerResize _playerResize;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private bool _hasDoubleJump;
    private bool _canDoubleJump;

    [HideInInspector]
    public bool canMove = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _playerResize = GetComponent<PlayerResize>();
        _playerControls = new PlayerControls();
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _moveSpeed = _baseMoveSpeed;
    }

    private void Start()
    {
        transform.localScale = new Vector3(3, 3, 3);
        ApplyUpgrades();
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _playerControls.Gameplay.Move.performed += OnMovePerformed;
        _playerControls.Gameplay.Move.canceled += OnMoveCanceled;
        _playerControls.Gameplay.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        _playerControls.Disable();
        _playerControls.Gameplay.Move.performed -= OnMovePerformed;
        _playerControls.Gameplay.Move.canceled -= OnMoveCanceled;
        _playerControls.Gameplay.Jump.performed -= OnJumpPerformed;
    }

    private void Update()
    {
        CheckGrounded();
        UpdateAnimations();

        if (_isGrounded)
        {
            _canDoubleJump = true;
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void UpdateAnimations()
    {
        if (_animator == null) return;

        float speed = Mathf.Abs(_moveInput);
        _animator.SetFloat("Speed", speed);
        _animator.SetBool("IsGrounded", _isGrounded);

        if (_isGrounded && _rb.velocity.y <= 0)
        {
            _animator.SetBool("Jump", false);
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        Vector2 input = context.ReadValue<Vector2>();
        _moveInput = input.x;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = 0f;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (!canMove) return;

        if (_isGrounded)
        {
            PerformJump();
        }
        else if (_hasDoubleJump && _canDoubleJump)
        {
            _canDoubleJump = false;
            PerformJump();
        }
    }

    private void PerformJump()
    {
        float currentScale = _playerResize.CurrentScale;

        float minScale = 1f;
        float maxScale = 3f;
        float t = (currentScale - minScale) / (maxScale - minScale);
        float jumpMultiplier = Mathf.Lerp(_minJumpMultiplier, _maxJumpMultiplier, t);
        float jumpForce = _baseJumpForce * jumpMultiplier;

        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);

        if (_animator != null)
        {
            _animator.SetBool("Jump", true);
        }
    }

    private void Move()
    {
        Vector2 velocity = new Vector2(_moveInput * _moveSpeed, _rb.velocity.y);
        _rb.velocity = velocity;

        if (_moveInput > 0)
        {
            _spriteRenderer.flipX = false;
        }
        else if (_moveInput < 0)
        {
            _spriteRenderer.flipX = true;
        }
    }

    private void CheckGrounded()
    {
        if (_collider == null) return;

        float colliderBottom = _collider.bounds.min.y;
        Vector2 groundCheckPoint = new Vector2(transform.position.x, colliderBottom);

        _isGrounded = Physics2D.OverlapCircle(groundCheckPoint, _groundCheckRadius, _groundLayer);
    }

    private void ApplyUpgrades()
    {
        if (UpgradeManager.Instance != null)
        {
            _moveSpeed = UpgradeManager.Instance.GetMoveSpeedBonus();
            _hasDoubleJump = UpgradeManager.Instance.doubleJumpUnlocked == 1;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (_collider != null)
        {
            float colliderBottom = _collider.bounds.min.y;
            Vector2 groundCheckPoint = new Vector2(transform.position.x, colliderBottom);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint, _groundCheckRadius);
        }
    }
}