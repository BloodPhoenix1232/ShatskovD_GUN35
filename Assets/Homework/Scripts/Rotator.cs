using System.Collections;
using UnityEngine;

internal class Rotator : MonoBehaviour
{
    [SerializeField]
    private Vector3 _rotate;
    [SerializeField]
    private float _rotationTime;

    private Rigidbody _rigidbody;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
        while (true)
        {
            transform.Rotate(_rotate * _rotationTime * Time.deltaTime);
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
