using UnityEngine;

public class PinSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _pinPrefab;
    [SerializeField] private Transform[] _spawnPoints;

    public void Start()
    {
        SpawnPins();
    }

    public void SpawnPins()
    {
        foreach (Transform point in _spawnPoints)
        {
            Instantiate(_pinPrefab, point.position, point.rotation);
        }
    }

    public void ClearPins()
    {
        GameObject[] oldPins = GameObject.FindGameObjectsWithTag("Pin");
        foreach (var pin in oldPins)
        {
            Destroy(pin);
        }
    }
}
