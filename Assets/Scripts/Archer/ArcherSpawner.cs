using UnityEngine;
using Netologia.Systems;

public class ArcherSpawner : MonoBehaviour
{
    public GameObject archerPrefab;
    public float interval = 5f;

    [Header("Системы")]
    public UnitSystem unitSystem;
    public ProjectileSystem projectileSystem;

    void Start()
    {
        InvokeRepeating(nameof(Spawn), 0f, interval);
    }

    void Spawn()
    {
        var archerGO = Instantiate(archerPrefab, transform.position, Quaternion.identity);
        var archer = archerGO.GetComponent<Archer>();
        archer.Initialize(unitSystem, projectileSystem);
    }
}