using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour
{
	[SerializeField]
    private Vector3 _localStart;
    [SerializeField] 
	private Vector3 _localEnd;
	[SerializeField]
	private float _speed;
	[SerializeField]
	private float _delay;

    private Vector3 _start;
    private Vector3 _end;
    private void OnValidate()
    {
        _start = transform.TransformPoint(_localStart);
        _end = transform.TransformPoint(_localEnd);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
		Gizmos.DrawLine(_end, _start);
		Gizmos.DrawSphere(_start, 0.25f);
		Gizmos.DrawSphere(_end, 0.25f);
    }

    private IEnumerator Start()
    {
        var tParam = 0f;
		var transform = this.transform;
        _start = transform.TransformPoint(_localStart);
        _end = transform.TransformPoint(_localEnd);

        while (true)
		{
			tParam += Time.deltaTime * _speed / Vector3.Distance(_start, _end);
			transform.localPosition = Vector3.Lerp(_start, _end, tParam);	
			if(tParam >= 1f)
			{
				tParam = 0f;
                (_start, _end) = (_end, _start);
                yield return new WaitForSeconds(_delay);
            }
			yield return null;
		}
	}
}
