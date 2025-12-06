using UnityEngine;

public class CameraScript : MonoBehaviour
{
    void OnDrawGizmos()
    {
        float verticalHeightSeen = GetComponent<Camera>().orthographicSize * 2.0f;
        float verticalWidthSeen = verticalHeightSeen * GetComponent<Camera>().aspect;


        Gizmos.color = Color.gray;
        Gizmos.DrawWireCube(transform.position, new Vector3(verticalWidthSeen, verticalHeightSeen, 0));
    }
}