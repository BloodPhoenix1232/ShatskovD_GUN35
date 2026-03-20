using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerClickHandler
{
    public Team Team;
    public UnitType Type;

    public Cell CurrentCell { get; private set; }

    [SerializeField] private float _heightOffset = 0.5f;

    public void SetCell(Cell cell)
    {
        CurrentCell = cell;
        if (cell != null)
        {
            transform.position = cell.transform.position + Vector3.up * _heightOffset;
        }
    }

    public void PromoteToQueen()
    {
        Type = UnitType.Queen;
        transform.localScale = new Vector3(0.8f, 0.25f, 0.8f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (CurrentCell != null)
        {
            CurrentCell.OnPointerClick(eventData);
        }
    }
}