using System.Collections.Generic;
using UnityEngine;

public class SpawnPlant : MonoBehaviour
{
    [SerializeField] private Plant _plant;
    [SerializeField] private List<SpawnPoint> _spawnPoints;
    [SerializeField] private Basket _basket;

    private List<Plant> _plants = new List<Plant>();
    private int currentSpawnPoint = 0;

    public List<Plant> GetPlants => _plants;

    public void AddSpawnPoint(List<SpawnPoint> spawnPoints)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            _spawnPoints.Add(spawnPoint);
        }
    }

    private void Start()
    {
        if (_plants.Count == 0)
        {
            StartSpawn();
        }
    }

    public void TryGetCountPoint(int countPoint)
    {
        currentSpawnPoint = countPoint;
    }

    public void Spawn()
    {
        foreach (SpawnPoint spawnPoint in _spawnPoints)
        {
            if (spawnPoint.IsBusy == false)
            {
                Plant plant = Instantiate(_plant, FindFreeSpawnPoint().transform.position, _plant.transform.rotation);

                plant.Init(_basket);
                _plants.Add(plant);

                plant.GetSpawnPoint(FindFreeSpawnPoint());
                FindFreeSpawnPoint().GetPlant(_plants[_plants.Count - 1]);
                currentSpawnPoint++;
                break;
            }
        }
    }

    private void StartSpawn()
    {
        Plant plant = Instantiate(_plant, _spawnPoints[currentSpawnPoint].transform.position, _plant.transform.rotation);

        plant.Init(_basket);
        _plants.Add(plant);

        _plants[_plants.Count - 1].GetSpawnPoint(_spawnPoints[currentSpawnPoint]);

        _spawnPoints[currentSpawnPoint].GetPlant(_plants[_plants.Count - 1]);
    }

    public void DecreaseSpawnPoint()
    {
        currentSpawnPoint--;
    }

    private SpawnPoint FindFreeSpawnPoint()
    {
        SpawnPoint spawnPoint = null;

        for (int i = 0; i < _spawnPoints.Count; i++)
        {
            if (_spawnPoints[i].IsBusy == false)
            {
                spawnPoint = _spawnPoints[i];
                break;
            }
        }
        return spawnPoint;
    }
}
