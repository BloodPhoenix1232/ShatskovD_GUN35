using UnityEngine;

public class Button : MonoBehaviour
{
    [SerializeField] private GameObject _door;
    [SerializeField] private float _doorMoveDistance = 3f;
    [SerializeField] private float _doorMoveSpeed = 2f;
    [SerializeField] private Sprite _pressedSprite;
    [SerializeField] private AudioClip _pressSound;
    [SerializeField] private AudioClip _doorOpenedSound;

    private Vector3 _doorStartPosition;
    private Vector3 _doorTargetPosition;
    private SpriteRenderer _spriteRenderer;
    private Sprite _defaultSprite;
    private bool _isPressed;
    private bool _doorMoving;
    private AudioSource _audioSource;

    private void Start()
    {
        if (_door != null)
        {
            _doorStartPosition = _door.transform.position;
            _doorTargetPosition = _doorStartPosition + Vector3.up * _doorMoveDistance;
        }

        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer != null)
        {
            _defaultSprite = _spriteRenderer.sprite;
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null && _pressSound != null)
        {
            _audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (_doorMoving && _door != null)
        {
            _door.transform.position = Vector3.MoveTowards(
                _door.transform.position,
                _doorTargetPosition,
                _doorMoveSpeed * Time.deltaTime);

            if (_door.transform.position == _doorTargetPosition)
            {
                _doorMoving = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isPressed) return;

        if (other.TryGetComponent<PlayerController>(out _))
        {
            _isPressed = true;

            if (_pressSound != null)
            {
                AudioManager.Instance?.PlaySFX(_pressSound);
                AudioManager.Instance?.PlaySFX(_doorOpenedSound);
            }

            if (_spriteRenderer != null && _pressedSprite != null)
            {
                _spriteRenderer.sprite = _pressedSprite;
            }

            if (_door != null)
            {
                _doorMoving = true;
            }
        }
    }
}