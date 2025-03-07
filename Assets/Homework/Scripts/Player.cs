using System.Collections;
using UnityEngine;

namespace Netologia.Homework
{
    public class Player : MonoBehaviour
    {
        private bool _ready;
        private Ball _ball;

        [SerializeField]
        private Ball _ballPrefab;
        [SerializeField]
        private float _startVelocity;
        [SerializeField]
        private float _lifetime;
        [SerializeField]
        private float _respawnDelay;

        private void Update()
        {

            if (!_ready) return;

            _ball.transform.position = transform.position + transform.forward * 1.5f;
            if (Input.GetKey(KeyCode.Space))
            {
                StartCoroutine(Reloader());

                
                _ball.transform.parent = null;
                _ball.Initialize(_startVelocity, _lifetime, transform.forward);
                
            }
        }

        private IEnumerator Reloader()
        {
            _ready = false;
            yield return new WaitForSeconds(_respawnDelay);
            Spawn();
        }

        private void Spawn()
        {
            Ball ballObject = Instantiate(_ballPrefab, transform.position + transform.forward * 1.5f, Quaternion.identity);

            _ball = ballObject.GetComponent<Ball>();
            _ready = true;
        }

        private void Start()
        {
            Spawn();
        }
    }
}
