using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Unit : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public Cell _currentCell { get; set; }

    public event Action OnMoveEndCallback;

    public void Move(Cell cell)
    {
        _currentCell = cell;
        StartCoroutine(MoveToCell(cell.transform.position));
    }

    private IEnumerator MoveToCell(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / 1f; // Равномерная скорость
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition;
        OnMoveEndCallback?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _currentCell?.OnPointerEnter(eventData);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        _currentCell?.OnPointerEnter(eventData);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _currentCell?.OnPointerEnter(eventData);
    }
}
