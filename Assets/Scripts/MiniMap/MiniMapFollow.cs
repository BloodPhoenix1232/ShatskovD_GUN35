using UnityEngine;

public class MiniMapFollow : MonoBehaviour
{
    [Header("Player & Offset")]
    public Transform player;                 // игрок
    public float height = 50f;               // высота камеры над игроком

    void LateUpdate()
    {
        if (player != null)
        {
            // Камера строго над игроком, смотрит вниз
            Vector3 newPos = new Vector3(player.position.x, player.position.y + height, player.position.z);
            transform.position = newPos;
            transform.rotation = Quaternion.Euler(90f, 0f, 0f); // смотрим строго вниз
        }
    }
}