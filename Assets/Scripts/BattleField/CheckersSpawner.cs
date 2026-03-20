using UnityEngine;

public class CheckersSpawner : MonoBehaviour
{
    public Battlefield battlefield;
    public GameObject whiteUnitPrefab;
    public GameObject blackUnitPrefab;

    private void Start()
    {
        if (battlefield == null)
        {
            battlefield = FindObjectOfType<Battlefield>();
        }

        SpawnUnits();
    }

    private void SpawnUnits()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 3; z++)
            {
                if ((x + z) % 2 == 1)
                {
                    SpawnUnit(blackUnitPrefab, x, z, Team.Black);
                }
            }
        }

        for (int x = 0; x < 8; x++)
        {
            for (int z = 5; z < 8; z++)
            {
                if ((x + z) % 2 == 1)
                {
                    SpawnUnit(whiteUnitPrefab, x, z, Team.White);
                }
            }
        }
    }

    private void SpawnUnit(GameObject prefab, int x, int z, Team team)
    {
        Cell cell = battlefield.GetCell(x, z);
        if (cell == null) return;

        GameObject obj = Instantiate(prefab);
        Unit unit = obj.GetComponent<Unit>();

        if (unit != null)
        {
            unit.Team = team;
            unit.Type = UnitType.Checker;
            cell.SetUnit(unit);
        }
    }
}