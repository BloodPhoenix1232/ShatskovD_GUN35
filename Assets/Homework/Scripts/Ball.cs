using UnityEngine;

namespace Netologia.Homework
{
    public class Ball : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public void Initialize(float startVelocity, float lifetime, Vector3 direction)
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (_rigidbody == null)
            {
                Debug.LogError("Ball: Rigidbody not found!");
                return;
            }

            _rigidbody.isKinematic = false;
            _rigidbody.velocity = direction * startVelocity;

            Destroy(gameObject, lifetime);
        }
    }
}
