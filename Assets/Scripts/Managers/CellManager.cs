using System;
using System.Collections.Generic;
using UnityEngine;
using primitives;

public class CellManager : MonoBehaviour
{
    private Dictionary<CellNeighbour, Cell> _neighbours;
    private Cell[] _cells;
    private Unit[] _units;

    public event Action<Cell> OnCellClicked;

    private void Awake()
    {
        _cells = FindObjectsOfType<Cell>();
        _neighbours = new Dictionary<CellNeighbour, Cell>(_cells.Length * 8);
        var positions = Array.ConvertAll(_cells, t => t.transform.position);
        var distance = 0f;

        for(int i = 0, iMax = _cells.Length; i < iMax; i++)
        {
            _cells[i].OnPointerClickEvent += OnCellClicked;

            for (int j = 0, jMax = _cells.Length; j < jMax; j++)
            {
                if (j == i) continue;
                var source = positions[i];
                var destination = positions[j];

                var forward = destination.z.CompareTo(source.z);
                var right = destination.x.CompareTo(source.x);
                var type = (forward, right) switch
                {
                    (1, 1) => NeighbourType.TopRight,
                    (1, 0) => NeighbourType.Top,
                    (1, -1) => NeighbourType.TopLeft,
                    (0, 1) => NeighbourType.Right,
                    (0, -1) => NeighbourType.Left,
                    (-1, 1) => NeighbourType.BottomRight,
                    (-1, 0) => NeighbourType.Bottom,
                    (-1, -1) => NeighbourType.BottomLeft,
                    _ => default
                };
                var key = new CellNeighbour(type, _cells[i]);
                var check = _neighbours.TryGetValue(key, out var cell)
                    ? Vector3.Distance(source, cell.transform.position)
                    : float.MaxValue;

                distance = Vector3.Distance(source, destination);
                if (distance < check)
                {
                    _neighbours[key] = _cells[j];
                }
            }
        }

        var units = FindObjectsOfType<Unit>();
        for(int i = 0, iMax = units.Length, index; i < iMax; i++)
        {
            (distance, index) = (float.MaxValue, -1);
            var position = units[i].transform.position;
            for (int j = 0, jMax = positions.Length; j < jMax; j++)
            {
                var calc = Vector3.Distance(position, positions[j]);
                if (calc < distance)
                {
                    (distance, index) = (calc, j);
                }
            }

            units[i]._currentCell = _cells[index];
            _cells[index].Unit = units[i];
        }
    }
}
