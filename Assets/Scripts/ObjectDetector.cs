using UnityEngine;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _distance;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private LayerMask _obstacleMask;
    [SerializeField] private LayerMask _trashMask;

    private Rigidbody _rigidBody;
    private bool _isTurning = false;
    private float _targetY;

    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Ray ray1 = new Ray(transform.position, Quaternion.Euler(0, 30, 0) * transform.forward);
        Ray ray2 = new Ray(transform.position, Quaternion.Euler(0, -30, 0) * transform.forward);
        Ray ray3 = new Ray(transform.position, transform.forward);

        Debug.DrawRay(transform.position, Quaternion.Euler(0, 30, 0) * transform.forward * _distance, Color.red);
        Debug.DrawRay(transform.position, Quaternion.Euler(0, -30, 0) * transform.forward * _distance, Color.red);
        Debug.DrawRay(transform.position, transform.forward * _distance, Color.red);

        Vector3 velocity = _rigidBody.velocity;

        if (_isTurning)
        {
            _rigidBody.velocity = Vector3.zero;

            float currentY = _rigidBody.rotation.eulerAngles.y;
            float newY = Mathf.MoveTowardsAngle(currentY, _targetY, _turnSpeed * Time.fixedDeltaTime);
            _rigidBody.MoveRotation(Quaternion.Euler(0, newY, 0));

            if (Mathf.Abs(Mathf.DeltaAngle(newY, _targetY)) < 0.5f)
            {
                _rigidBody.MoveRotation(Quaternion.Euler(0, _targetY, 0));
                _isTurning = false;
            }

            return;
        }


        if (Physics.Raycast(ray1, _distance, _obstacleMask) || Physics.Raycast(ray2, _distance, _obstacleMask) || Physics.Raycast(ray3, _distance, _obstacleMask))
        {
            _isTurning = true;
            float randomTurn = Random.value < 0.5f ? -90f : 90f;
            _targetY = transform.eulerAngles.y + randomTurn;
        }
        else
        {
            velocity.x = transform.forward.x * _speed;
            velocity.z = transform.forward.z * _speed;
            _rigidBody.velocity = velocity;

            CollectTrash();
        }
    }

    void CollectTrash()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 1f, _trashMask);

        foreach (Collider hit in hits)
        {
            Destroy(hit.gameObject);
        }
    }
}
