using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class Trajectory : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private int _segmentCount = 20;
    [SerializeField] private float _timeStep = 0.05f;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Rigidbody2D _playerRb;

    private Vector2 _startPosition;
    private Vector2 _startVelocity;
    private float _gravity;
    private bool _isCharging;
    private float _currentPower;
    private float _minPower = 5f;
    private float _maxPower = 15f;
    private float _minChargeTime = 0.5f;
    private float _maxChargeTime = 2f;
    private float _chargeStartTime;

    private void Start()
    {
        _gravity = Mathf.Abs(Physics2D.gravity.y);

        if (_lineRenderer != null)
        {
            _lineRenderer.enabled = false;
        }
    }

    private void Update()
    {
        if (_isCharging)
        {
            float chargeTime = Time.time - _chargeStartTime;
            float charge = Mathf.Clamp01(chargeTime / _maxChargeTime);
            _currentPower = Mathf.Lerp(_minPower, _maxPower, charge);

            DrawTrajectory();
        }
    }

    public void StartCharge(float chargeStartTime)
    {
        _isCharging = true;
        _chargeStartTime = chargeStartTime;

        if (_lineRenderer != null)
        {
            _lineRenderer.enabled = true;
        }
    }

    public void EndCharge()
    {
        _isCharging = false;

        if (_lineRenderer != null)
        {
            _lineRenderer.enabled = false;
        }
    }

    private void DrawTrajectory()
    {
        if (_lineRenderer == null || _shootPoint == null) return;

        SpriteRenderer playerSprite = GetComponent<SpriteRenderer>();
        float direction = playerSprite != null && playerSprite.flipX ? -1f : 1f;

        _startPosition = _shootPoint.position;
        _startVelocity = new Vector2(direction * _currentPower, _currentPower * 0.5f);

        List<Vector3> points = new List<Vector3>();
        Vector2 position = _startPosition;
        Vector2 velocity = _startVelocity;
        points.Add(position);

        for (int i = 0; i < _segmentCount; i++)
        {
            velocity.y -= _gravity * _timeStep;
            position += velocity * _timeStep;
            points.Add(position);

            if (position.y < -10f) break;
        }

        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    public void SetChargeParameters(float minPower, float maxPower, float minChargeTime, float maxChargeTime)
    {
        _minPower = minPower;
        _maxPower = maxPower;
        _minChargeTime = minChargeTime;
        _maxChargeTime = maxChargeTime;
    }
}