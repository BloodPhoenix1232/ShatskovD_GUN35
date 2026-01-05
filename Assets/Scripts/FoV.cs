using UnityEditor;
using UnityEngine;

public class FoV : MonoBehaviour
{
    [SerializeField]
    private float _radius;

    [SerializeField, Range(0, 360)]
    private float _angle;

    private Transform _transform;
    private Vector3 _centerOfView;

    private void OnDrawGizmos()
    {
        _transform = transform;
        _centerOfView = _transform.position;

        Gizmos.color = Color.red;
        Handles.color = Color.red;

        Vector3 viewAngle1 = DirectionFromAngle(_angle / 2);
        Vector3 viewAngle2 = DirectionFromAngle(-_angle / 2);

        Handles.DrawWireArc(_centerOfView, Vector3.up, viewAngle2, _angle, _radius);
        Gizmos.DrawLine(_centerOfView, _transform.position + viewAngle1 * _radius);
        Gizmos.DrawLine(_centerOfView, _transform.position + viewAngle2 * _radius);
    }

    private Vector3 DirectionFromAngle(float angle)
    {
        float rad = (angle + transform.eulerAngles.y) * Mathf.Deg2Rad;

        return new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
    }
}
