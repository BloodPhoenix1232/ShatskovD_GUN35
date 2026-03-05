using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
    [SerializeField] private SpawnPlant _spawnPlant;
    [SerializeField] private Merger _merger;

    private Plant _selectedObject;
    private BoxCollider _collider;
    private Vector3 _newPosition;
    private RaycastHit _hit;
    private int _countMerge = 0;
    private bool _isTaken = false;

    public int CountMerge => _countMerge;
    public bool IsTaken => _isTaken;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_selectedObject == null)
            {
                if (Input.mousePosition != null)
                {
                    _hit = CastRayForComputers();
                }
                else if (Input.GetTouch(0).tapCount > 0)
                {
                    _hit = CastRayForPhones();
                }

                if (_hit.collider != null)
                {
                    if (_hit.collider.TryGetComponent(out Plant plant))
                    {
                        _selectedObject = plant;
                        _collider = plant.gameObject.GetComponent<BoxCollider>();
                        return;
                    }
                }
            }
            else
            {
                _merger.TryMerge(_selectedObject, _collider);
            }
        }

        if (_selectedObject != null)
        {
            if (Input.mousePosition != null)
            {
                MovingSelectedObject();
            }


            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Ended)
                {
                    _merger.TryMerge(_selectedObject, _collider);
                    PutPlant(_selectedObject.SpawnPoint);
                }
            }
        }
    }

    private void MovingSelectedObject()
    {
        _selectedObject.StopSpawn();

        _selectedObject.SpawnPoint.DeletePlant();
        _collider.enabled = false;

        Vector3 position = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.WorldToScreenPoint(_selectedObject.transform.position).z
        );

        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        _selectedObject.transform.position = new Vector3(worldPosition.x, 1f, worldPosition.z);
        _selectedObject.TakePlant();
        _isTaken = true;
    }

    private RaycastHit CastRayForComputers()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);

        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        Debug.DrawRay(worldMousePosNear, (worldMousePosFar - worldMousePosNear) * 100, Color.red);

        return hit;
    }

    private RaycastHit CastRayForPhones()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.GetTouch(0).position.x,
            Input.GetTouch(0).position.y,
            Camera.main.farClipPlane);

        Vector3 screenMousePosNear = new Vector3(
            Input.GetTouch(0).position.x,
            Input.GetTouch(0).position.y,
            Camera.main.nearClipPlane);  

        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);
        Debug.DrawRay(worldMousePosNear, (worldMousePosFar - worldMousePosNear) * 100, Color.red);

        return hit;
    }

    public void PutPlant(SpawnPoint spawnPoint)
    {
        _selectedObject.StartSpawn();

        _selectedObject.GetSpawnPoint(spawnPoint);

        _newPosition = spawnPoint.transform.position;
        _selectedObject.transform.position = _newPosition;

        spawnPoint.GetPlant(_selectedObject);
        _selectedObject.Release();
        _collider.enabled = true;
        _selectedObject = null;
        _isTaken = false;
    }
}
