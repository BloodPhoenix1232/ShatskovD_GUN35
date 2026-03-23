using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Jump Settings")]
    [SerializeField] private float _baseJumpForce = 10f;
    [SerializeField] private float _minJumpMultiplier = 0.5f;  // для маленького размера
    [SerializeField] private float _maxJumpMultiplier = 1.5f;  // для большого размера
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private float _moveInput;
    private Rigidbody2D _rb;
    private PlayerControls _playerControls;
    private bool _isGrounded;
    private Collider2D _collider;
    private PlayerResize _playerResize;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        _playerResize = GetComponent<PlayerResize>();
        _playerControls = new PlayerControls();
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
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        _moveInput = input.x;
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        _moveInput = 0f;
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (_isGrounded)
        {
            float jumpMultiplier = Mathf.Lerp(_minJumpMultiplier, _maxJumpMultiplier, _playerResize.CurrentScale);
            float jumpForce = _baseJumpForce * jumpMultiplier;

            _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        }
    }

    private void Move()
    {
        Vector2 velocity = new Vector2(_moveInput * _moveSpeed, _rb.velocity.y);
        _rb.velocity = velocity;
    }

    private void CheckGrounded()
    {
        if (_collider == null) return;

        float colliderBottom = _collider.bounds.min.y;
        Vector2 groundCheckPoint = new Vector2(transform.position.x, colliderBottom);

        _isGrounded = Physics2D.OverlapCircle(groundCheckPoint, _groundCheckRadius, _groundLayer);
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