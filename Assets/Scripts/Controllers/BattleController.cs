using UnityEngine;
using System;

public class BattleController : MonoBehaviour
{
    public static BattleController Instance;
    public static Team CurrentTurn = Team.White;

    public Battlefield Battlefield;
    private IGameplayCommand command;

    public event Action<Team> OnGameEnded;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (Battlefield != null)
        {
            command = new CheckersCommand(Battlefield);
        }
    }

    public void OnCellClicked(Cell cell)
    {
        if (command != null)
        {
            command.Interact(cell);
        }
    }

    public void EndTurn()
    {
        if (Battlefield != null)
        {
            for (int x = 0; x < 8; x++)
            {
                for (int z = 0; z < 8; z++)
                {
                    Cell cell = Battlefield.GetCell(x, z);
                    if (cell != null)
                    {
                        cell.SetHighlight(false);
                        cell.ResetSelect();
                    }
                }
            }
        }

        CurrentTurn = CurrentTurn == Team.White ? Team.Black : Team.White;
    }

    public void CheckWinAfterMove()
    {
        int whiteCount = 0;
        int blackCount = 0;

        for (int x = 0; x < 8; x++)
        {
            for (int z = 0; z < 8; z++)
            {
                Cell cell = Battlefield.GetCell(x, z);
                if (cell != null && cell.Unit != null)
                {
                    if (cell.Unit.Team == Team.White)
                        whiteCount++;
                    else
                        blackCount++;
                }
            }
        }

        if (whiteCount == 0)
        {
            EndGame(Team.Black);
        }
        else if (blackCount == 0)
        {
            EndGame(Team.White);
        }
    }

    private void EndGame(Team winner)
    {
        Debug.Log($"Победа! {winner} выиграл!");
        OnGameEnded?.Invoke(winner);
        command = null;
    }
}