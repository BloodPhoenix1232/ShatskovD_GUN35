using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabber : MonoBehaviour
{
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
                else if(Input.GetTouch(0).tapCount > 0)
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
        }
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
}
