using UnityEngine;
using System.Collections.Generic;

public class CheckersCommand : IGameplayCommand
{
    private Unit selectedUnit;
    private Battlefield field;
    private bool mustCapture = false;

    public CheckersCommand(Battlefield battlefield)
    {
        field = battlefield;
    }

    public void Interact(Cell cell)
    {
        Team currentTurn = BattleController.CurrentTurn;

        if (selectedUnit == null)
        {
            if (cell.Unit != null && cell.Unit.Team == currentTurn)
            {
                selectedUnit = cell.Unit;
                cell.SetSelect();
                HighlightPossibleMoves(selectedUnit);
            }
            return;
        }

        if (cell == selectedUnit.CurrentCell)
        {
            selectedUnit.CurrentCell.ResetSelect();
            ClearHighlights();
            selectedUnit = null;
            return;
        }

        TryMove(cell);
    }

    private void HighlightPossibleMoves(Unit unit)
    {
        ClearHighlights();
        mustCapture = false;

        List<Cell> captureMoves = GetCaptureMoves(unit);

        if (captureMoves.Count > 0)
        {
            mustCapture = true;
            foreach (Cell cell in captureMoves)
            {
                cell.SetHighlight(true);
            }
        }

        if (!mustCapture)
        {
            List<Cell> normalMoves = GetNormalMoves(unit);
            foreach (Cell cell in normalMoves)
            {
                cell.SetHighlight(true);
            }
        }
    }

    private List<Cell> GetNormalMoves(Unit unit)
    {
        List<Cell> moves = new List<Cell>();
        Vector3Int pos = unit.CurrentCell.Position;

        if (unit.Type == UnitType.Queen)
        {
            int[] directions = { -1, 1 };

            foreach (int dx in directions)
            {
                foreach (int dz in directions)
                {
                    int step = 1;
                    while (true)
                    {
                        int newX = pos.x + dx * step;
                        int newZ = pos.z + dz * step;

                        if (newX < 0 || newX >= 8 || newZ < 0 || newZ >= 8)
                            break;

                        Cell cell = field.GetCell(newX, newZ);

                        if (cell.Unit == null)
                        {
                            moves.Add(cell);
                            step++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            int dir = unit.Team == Team.White ? -1 : 1;
            int[] xOffsets = { -1, 1 };

            foreach (int xOffset in xOffsets)
            {
                int newX = pos.x + xOffset;
                int newZ = pos.z + dir;

                if (newX >= 0 && newX < 8 && newZ >= 0 && newZ < 8)
                {
                    Cell cell = field.GetCell(newX, newZ);
                    if (cell != null && cell.Unit == null)
                    {
                        moves.Add(cell);
                    }
                }
            }
        }

        return moves;
    }

    private List<Cell> GetCaptureMoves(Unit unit)
    {
        List<Cell> captures = new List<Cell>();
        Vector3Int pos = unit.CurrentCell.Position;

        if (unit.Type == UnitType.Queen)
        {
            int[] directions = { -1, 1 };

            foreach (int dx in directions)
            {
                foreach (int dz in directions)
                {
                    int step = 1;
                    Cell enemyCell = null;

                    while (true)
                    {
                        int newX = pos.x + dx * step;
                        int newZ = pos.z + dz * step;

                        if (newX < 0 || newX >= 8 || newZ < 0 || newZ >= 8)
                            break;

                        Cell cell = field.GetCell(newX, newZ);

                        if (enemyCell == null)
                        {
                            if (cell.Unit != null && cell.Unit.Team != unit.Team)
                            {
                                enemyCell = cell;
                            }
                        }
                        else
                        {
                            if (cell.Unit == null)
                            {
                                captures.Add(cell);
                            }
                            break;
                        }

                        step++;
                    }
                }
            }
        }
        else
        {
            int dir = unit.Team == Team.White ? -1 : 1;
            int[] xOffsets = { -1, 1 };

            foreach (int xOffset in xOffsets)
            {
                int newX = pos.x + xOffset * 2;
                int newZ = pos.z + dir * 2;
                int midX = pos.x + xOffset;
                int midZ = pos.z + dir;

                if (newX >= 0 && newX < 8 && newZ >= 0 && newZ < 8)
                {
                    Cell midCell = field.GetCell(midX, midZ);
                    Cell targetCell = field.GetCell(newX, newZ);

                    if (midCell != null && targetCell != null &&
                        midCell.Unit != null &&
                        midCell.Unit.Team != unit.Team &&
                        targetCell.Unit == null)
                    {
                        captures.Add(targetCell);
                    }
                }
            }
        }

        return captures;
    }

    private void TryMove(Cell targetCell)
    {
        Vector3Int from = selectedUnit.CurrentCell.Position;
        Vector3Int to = targetCell.Position;

        List<Cell> captureMoves = GetCaptureMoves(selectedUnit);

        if (captureMoves.Contains(targetCell))
        {
            if (selectedUnit.Type == UnitType.Queen)
            {
                int dirX = to.x > from.x ? 1 : -1;
                int dirZ = to.z > from.z ? 1 : -1;

                int step = 1;
                while (true)
                {
                    int checkX = from.x + dirX * step;
                    int checkZ = from.z + dirZ * step;

                    if (checkX == to.x && checkZ == to.z)
                        break;

                    Cell checkCell = field.GetCell(checkX, checkZ);
                    if (checkCell != null && checkCell.Unit != null && checkCell.Unit.Team != selectedUnit.Team)
                    {
                        Object.Destroy(checkCell.Unit.gameObject);
                        checkCell.ClearUnit();
                    }
                    step++;
                }
            }
            else
            {
                int midX = (from.x + to.x) / 2;
                int midZ = (from.z + to.z) / 2;
                Cell midCell = field.GetCell(midX, midZ);

                if (midCell != null && midCell.Unit != null)
                {
                    Object.Destroy(midCell.Unit.gameObject);
                    midCell.ClearUnit();
                }
            }

            Move(targetCell);
        }
        else if (!mustCapture)
        {
            bool isValidMove = false;

            if (selectedUnit.Type == UnitType.Queen)
            {
                int deltaX = Mathf.Abs(to.x - from.x);
                int deltaZ = Mathf.Abs(to.z - from.z);
                isValidMove = (deltaX == deltaZ) && (deltaX > 0);

                if (isValidMove)
                {
                    int dirX = to.x > from.x ? 1 : -1;
                    int dirZ = to.z > from.z ? 1 : -1;

                    int step = 1;
                    bool pathClear = true;

                    while (step < deltaX)
                    {
                        int checkX = from.x + dirX * step;
                        int checkZ = from.z + dirZ * step;
                        Cell checkCell = field.GetCell(checkX, checkZ);

                        if (checkCell != null && checkCell.Unit != null)
                        {
                            pathClear = false;
                            break;
                        }
                        step++;
                    }

                    isValidMove = pathClear;
                }
            }
            else
            {
                int deltaX = Mathf.Abs(to.x - from.x);
                int deltaZ = Mathf.Abs(to.z - from.z);
                int dir = selectedUnit.Team == Team.White ? -1 : 1;
                isValidMove = (deltaX == 1 && deltaZ == 1 && to.z - from.z == dir);
            }

            if (isValidMove && targetCell.Unit == null)
            {
                Move(targetCell);
            }
        }
    }

    private void Move(Cell targetCell)
    {
        Cell fromCell = selectedUnit.CurrentCell;

        fromCell.ClearUnit();
        targetCell.SetUnit(selectedUnit);

        fromCell.ResetSelect();
        ClearHighlights();

        if (selectedUnit.Type != UnitType.Queen)
        {
            if ((selectedUnit.Team == Team.White && targetCell.Position.z == 0) ||
                (selectedUnit.Team == Team.Black && targetCell.Position.z == 7))
            {
                selectedUnit.PromoteToQueen();
            }
        }

        selectedUnit = null;

        BattleController.Instance.EndTurn();
        BattleController.Instance.CheckWinAfterMove();
    }

    private void ClearHighlights()
    {
        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                Cell cell = field.GetCell(x, z);
                if (cell != null)
                {
                    cell.SetHighlight(false);
                }
            }
        }
    }
}