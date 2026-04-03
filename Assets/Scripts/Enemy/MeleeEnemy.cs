using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _moveRange = 3f;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = 0.2f;

    [SerializeField] private int _damage = 1;
    [SerializeField] private float _damageCooldown = 1f;

    [SerializeField] private Animator _animator;

    private Rigidbody2D _rb;
    private float _leftBoundary;
    private float _rightBoundary;
    private bool _movingRight = true;
    private float _lastDamageTime;
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

        float speed = Mathf.Abs(_moveInput);
        _animator.SetFloat("Speed", speed);
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

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth health))
        {
            if (Time.time - _lastDamageTime >= _damageCooldown)
            {
                health.TakeDamage(_damage);
                _lastDamageTime = Time.time;
            }
        }
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