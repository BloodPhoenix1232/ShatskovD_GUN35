using UnityEngine;
using DG.Tweening;


public class WayPoint : MonoBehaviour
{
    [SerializeField]
    private Transform[] _wayPoints;
    [SerializeField]
    private float _speed = 2f;
    [SerializeField]
    private float _scale = 3f;
    [SerializeField]
    private Ease _easeMethod = Ease.Linear;
    [SerializeField]
    private LoopType _loopType = LoopType.Yoyo;

    private int _current = 0;

    void Start()
    {
        if (_wayPoints.Length == 0) return;

        MoveToPoint();
    }

    void MoveToPoint()
    {
        Transform target = _wayPoints[_current];
        float duration = Vector3.Distance(transform.position, target.position) / _speed;

        DOTween.Sequence().Append(transform.DOMove(target.position, duration).SetEase(_easeMethod).SetDelay(1f))                        // Движение
            .Join(transform.DOScale(_scale, duration).SetLoops(2, _loopType).SetEase(_easeMethod))                                      // Смена размера
            .Join(transform.GetComponent<Renderer>().material.DOColor(new Color(1f, 0f, 0f), duration).SetLoops(2, _loopType))          // Смена цвета
            .OnComplete(() =>
            {
                _current++;
                if (_current >= _wayPoints.Length)
                    _current = 0;

                MoveToPoint();
            });
    }
}
