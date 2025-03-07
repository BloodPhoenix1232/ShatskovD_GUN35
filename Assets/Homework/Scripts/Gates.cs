using UnityEngine;

namespace Netologia.Homework
{
    public class Gates : MonoBehaviour
    {
        private BoxCollider _collider;
        private int _score;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            Ball ball = other.GetComponentInParent<Ball>();

            if (ball != null)
            {
                _score += 1;
                Debug.Log($"Текущий счет: {_score}");
                Destroy(other.gameObject);
            }
        }
    }
}
