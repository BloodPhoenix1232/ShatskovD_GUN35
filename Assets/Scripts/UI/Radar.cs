using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Radar : MonoBehaviour
{
    private readonly float _mapScale = 2;
    private Transform _playerPos;
    public static List<RadarObject> RadObjects = new();

    private void Start()
    {
        _playerPos = Camera.main.transform;
    }

    public static void RegisterRadarObject(GameObject obj, Image im)
    {
        var image = Instantiate(im);
        RadObjects.Add(new RadarObject { Owner = obj, Icon = image });
    }

    public static void RemoveRadarObject(GameObject obj)
    {
        List<RadarObject> newList = new List<RadarObject>();
        foreach (RadarObject t in RadObjects)
        {
            if (t.Owner == obj)
            {
                Destroy(t.Icon);
                continue;
            }
            newList.Add(t);
        }
        RadObjects.RemoveRange(0, RadObjects.Count);
        RadObjects.AddRange(newList);
    }

    private void DrawRadarDots()
    {
        foreach (RadarObject radarObject in RadObjects)
        {
            Vector3 radarPos = (radarObject.Owner.transform.position - _playerPos.position);
            float distToObject = Vector3.Distance(_playerPos.position, radarObject.Owner.transform.position) * _mapScale;
            float deltay = Mathf.Atan2(radarPos.x, radarPos.z) * Mathf.Rad2Deg - 270 - _playerPos.eulerAngles.y;

            radarPos.x = distToObject * Mathf.Cos(deltay *  Mathf.Deg2Rad) * -1;
            radarPos.z = distToObject * Mathf.Sin(deltay * Mathf.Deg2Rad);

            radarObject.Icon.transform.SetParent(transform);
            radarObject.Icon.transform.position = new Vector3(radarPos.x, radarPos.z, 0) + transform.position;
        }
    }

    private void Update()
    {
        if(Time.frameCount % 2 == 0)
        { 
            DrawRadarDots(); 
        }
    }

    public sealed class RadarObject
    {
        public Image Icon;
        public GameObject Owner;
    }
}
