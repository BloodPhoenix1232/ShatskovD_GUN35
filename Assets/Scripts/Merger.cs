using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merger : MonoBehaviour
{
    [SerializeField] private SpawnPlant _spawnPlant;
    [SerializeField] private Grabber _grabber;

    private int _countMerge = 0;
    private Vector3 _newPosition;
    private BoxCollider _collider;

    public bool TryMerge(Plant selectedObject, BoxCollider _collider)
    {
        bool _isMerge = false;
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.gameObject.TryGetComponent(out Plant plant) && plant != selectedObject)
            {
                Merge(plant, selectedObject);
                _isMerge = true;
            }
            else if (hit.collider.gameObject.TryGetComponent(out SpawnPoint spawnPoint) && (spawnPoint.IsBusy == false))
            {
                _grabber.PutPlant(spawnPoint);
            }
        }

        return _isMerge;
    }

    private void Merge(Plant plant, Plant selectedObject)
    {
        if (TryFindIdenticalFruits(plant, selectedObject))
        {
            _countMerge++;
            //plant.PlayEffect();
            _spawnPlant.DecreaseSpawnPoint();
            Destroy(selectedObject.gameObject);
            selectedObject = null;


            plant.ImprovePlantModel();
            plant.ModificationFruit();
            plant.SpawnPoint.GetPlant(plant);
        }
    }

    private bool TryFindIdenticalFruits(Plant plant, Plant selectedObject)
    {
        if (plant.CurrentIndexFruit == selectedObject.CurrentIndexFruit)
        {
            return true;
        }
        return false;
    }
}
