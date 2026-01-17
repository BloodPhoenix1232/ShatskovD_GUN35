using System.Collections;
using UnityEngine;

public class BallLaunch : MonoBehaviour
{
    private bool _ready = true;

    [SerializeField] 
    private GameObject _ballPrefab;
    [SerializeField] 
    private float _lifeTime = 5f;
    [SerializeField] 
    private float _startVelocity = 10f;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && GameManager.Instance.ReadyToLaunch())
        {
            Laucnh();
        }
    }

    void Laucnh()
    {
        if (_ready)
        {
            GameObject ball = Instantiate(_ballPrefab, transform.position + transform.forward, Quaternion.identity);

            Rigidbody rigidBody = ball.GetComponent<Rigidbody>();
            rigidBody.velocity = transform.forward * _startVelocity;

            GameManager.Instance.StartThrow();
            StartCoroutine(EndThrowDelay());

            Destroy(ball, _lifeTime);
        }
    }

    private IEnumerator EndThrowDelay()
    {
        yield return new WaitForSeconds(4f);
        GameManager.Instance.EndThrow();
    }
}
