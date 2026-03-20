using UnityEngine;

public class Battlefield : MonoBehaviour
{
    public Cell[,] Cells = new Cell[8, 8];

    private void Awake()
    {
        Cell[] allCells = FindObjectsOfType<Cell>();

        foreach (Cell cell in allCells)
        {
            Vector3 worldPos = cell.transform.position;
            int x = Mathf.RoundToInt(worldPos.x);
            int z = Mathf.RoundToInt(worldPos.z);

            if (x >= 0 && x < 8 && z >= 0 && z < 8)
            {
                Cells[x, z] = cell;
                cell.Init(new Vector3Int(x, 0, z));
            }
        }
    }

    public Cell GetCell(int x, int z)
    {
        if (x < 0 || x >= 8 || z < 0 || z >= 8)
            return null;
        return Cells[x, z];
    }
}