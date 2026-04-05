using UnityEngine;

public class MeleeEnemy : Enemy
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moveRange = 3f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private Animator _animator;

    private Rigidbody2D _rb;
    private float _leftBoundary;
    private float _rightBoundary;
    private bool _movingRight = true;
    private float _moveInput;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_animator == null)
            _animator = GetComponent<Animator>();

        _leftBoundary = transform.position.x - _moveRange;
        _rightBoundary = transform.position.x + _moveRange;
    }

    private void Update()
    {
        CheckGrounded();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (_movingRight)
        {
            _moveInput = 1f;
            transform.localScale = new Vector3(1, 1, 1);

            if (transform.position.x >= _rightBoundary)
            {
                _movingRight = false;
                _moveInput = 0f;
            }
        }
        else
        {
            _moveInput = -1f;
            transform.localScale = new Vector3(-1, 1, 1);

            if (transform.position.x <= _leftBoundary)
            {
                _movingRight = true;
                _moveInput = 0f;
            }
        }

        Vector2 velocity = new Vector2(_moveInput * _moveSpeed, _rb.velocity.y);
        _rb.velocity = velocity;
    }

    private void UpdateAnimations()
    {
        if (_animator == null) return;
        _animator.SetFloat("Speed", Mathf.Abs(_moveInput));
    }

    private void CheckGrounded()
    {
        if (_groundCheck == null) return;
        bool isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        if (_animator != null)
        {
            _animator.SetBool("IsGrounded", isGrounded);
        }
    }

    public void TakeDamage(int damage)
    {
        Die();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float left = transform.position.x - _moveRange;
        float right = transform.position.x + _moveRange;
        Gizmos.DrawLine(new Vector3(left, transform.position.y), new Vector3(right, transform.position.y));

        if (_groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
        }
    }
}