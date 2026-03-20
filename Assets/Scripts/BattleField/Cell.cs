using UnityEngine;
using UnityEngine.EventSystems;

public class Cell : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private MeshRenderer _focus;
    [SerializeField] private MeshRenderer _select;
    [SerializeField] private MeshRenderer _highlight;

    public Vector3Int Position { get; private set; }
    public Unit Unit { get; private set; }

    public void Init(Vector3Int position)
    {
        Position = position;
    }

    public void SetUnit(Unit unit)
    {
        Unit = unit;
        if (unit != null)
            unit.SetCell(this);
    }

    public void ClearUnit()
    {
        Unit = null;
    }

    public void SetSelect()
    {
        if (_select != null)
            _select.enabled = true;
    }

    public void ResetSelect()
    {
        if (_select != null)
            _select.enabled = false;
    }

    public void SetHighlight(bool enabled)
    {
        if (_highlight != null)
            _highlight.enabled = enabled;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (BattleController.Instance != null)
        {
            BattleController.Instance.OnCellClicked(this);
        }
    }
}