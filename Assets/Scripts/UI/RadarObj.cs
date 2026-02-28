using UnityEngine;
using UnityEngine.UI;


public class RadarObj : MonoBehaviour
{
    [SerializeField] private Image _ico;

    private void OnValidate() => _ico = Resources.Load<Image>("Minimap/RadarObject");

    private void OnDisable() => Radar.RemoveRadarObject(gameObject);

    private void OnEnable() => Radar.RegisterRadarObject(gameObject, _ico);
}
