using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    private Transform _player;

    private Tilemap _map;
    private Camera _camera;

    private void Start()
    {
        _map = GetComponent<Tilemap>();
        _camera = Camera.main;

        if (_player == null)
        {
            Debug.LogError("Player не назначен в инспекторе!");
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Mathf.Abs(_camera.transform.position.z);
            Vector3 clickWorldPosition = _camera.ScreenToWorldPoint(mousePosition);

            Vector3Int clickCellPosition = _map.WorldToCell(clickWorldPosition);

            Vector3 cellCenterWorldPosition = _map.GetCellCenterWorld(clickCellPosition);

            _player.position = cellCenterWorldPosition;
        }
    }
}