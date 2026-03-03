using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private Fruit _currentFruit;
    [SerializeField] private List<GameObject> _fruitObject;
    [SerializeField] private GameObject _spawnPointFruit;
    [SerializeField] private float _spawnTime;
    [SerializeField] private List<GameObject> _prefabPoint;
    [SerializeField] Basket _basket;
    [SerializeField] private Grabber _grabber;

    private float _spawnTimeWas;

    private void Start()
    {
        _spawnTimeWas = _spawnTime;
    }

    private void Update()
    {
        if(_currentFruit == null)
        {
            _spawnTimeWas += Time.deltaTime;

            if(_spawnTimeWas > _spawnTime)
            {
                _spawnTimeWas = 0;

                _currentFruit = Instantiate(_fruitObject[0], _spawnPointFruit.transform.position, _fruitObject[0].transform.rotation).GetComponent<Fruit>();

                _currentFruit.Init(_basket);
                _currentFruit.transform.SetParent(transform, true);
                _currentFruit.GetComponent<FruitMovement>().SetParentPlant(this);
            }
        }
    }

    public void DeleteFruit()
    {
        _currentFruit = null;
    }
}
