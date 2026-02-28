using UnityEngine;

public sealed class PlayerBall : PlayerBase
{
    [SerializeField] private Rigidbody _rigidbody;

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        Move(direction);
    }

    protected override void Move(Vector3 direction)
    {
        _rigidbody.AddForce(direction * Speed);
    }
}