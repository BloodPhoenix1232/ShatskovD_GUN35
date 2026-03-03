using UnityEngine;

public class UITrail : MonoBehaviour
{
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] private TrailRenderer _trailRenderer;

    private Vector2 mousePosition;

    public void Update()
    {
        if (Input.mousePosition != null)
        {
            mousePosition = Input.mousePosition;
        }
        else if (Input.touchCount > 0)
        {
            mousePosition = Input.GetTouch(0).position;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, mousePosition, _canvas.worldCamera, out Vector2 canvasPos);
        _rectTransform.anchoredPosition = canvasPos;

        if (Input.GetMouseButton(0))
        {
            _trailRenderer.emitting = true;
        }
        else
        {
            _trailRenderer.emitting = false;
        }
    }
}
