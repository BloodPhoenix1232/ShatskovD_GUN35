using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 3;
    [SerializeField] private float _invincibilityDuration = 1f;

    private int _currentHealth;
    private bool _isInvincible;
    private float _invincibilityTimer;

    public int CurrentHealth => _currentHealth;
    public int MaxHealth => _maxHealth;

    public delegate void OnHealthChanged(int currentHealth, int maxHealth);
    public event OnHealthChanged HealthChanged;

    public delegate void OnPlayerDied();
    public event OnPlayerDied PlayerDied;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        if (_isInvincible)
        {
            _invincibilityTimer -= Time.deltaTime;
            if (_invincibilityTimer <= 0)
            {
                _isInvincible = false;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isInvincible) return;
        if (_currentHealth <= 0) return;
        
        _currentHealth -= damage;
        Debug.Log("Урон" + _currentHealth);
        HealthChanged?.Invoke(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }
        else
        {
            _isInvincible = true;
            _invincibilityTimer = _invincibilityDuration;
        }
    }

    private void Die()
    {
        PlayerDied?.Invoke();
    }
}