using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    [SerializeField] private Fruit _currentFruit;
    [SerializeField] private FruitObject _treeClass;
    [SerializeField] private List<FruitObject> _fruitObject;
    [SerializeField] private SpawnPoint _spawnPoint;
    [SerializeField] private GameObject _spawnPointFruit;
    [SerializeField] private float _spawnTime;
    [SerializeField] private int _currentIndexFruit;

    [SerializeField] private List<GameObject> _prefabPlant;
    [SerializeField] private int _currentIndexPlant;
    [SerializeField] private ParticleSystem _plantEffect;
    [SerializeField] private Grabber _grabber;

    private Basket _basket;
    private float _spawnTimeWas;
    private bool _isGrabbing = false;
    private int _emptyModel = 2;

    public Grabber Grabber => _grabber;
    public Basket Basket => _basket;
    public FruitObject TreeClass => _treeClass;
    public Fruit CurrentFruit => _currentFruit;
    public float SpawnTime => _spawnTime;
    public SpawnPoint SpawnPoint => _spawnPoint;
    public int CurrentIndexFruit => _currentIndexFruit;
    public int CurrentIndexPlant => _currentIndexPlant;

    private void Start()
    {
        _spawnTimeWas = _spawnTime;
    }
    private void Update()
    {
        if (_currentFruit == null && _isGrabbing == false)
        {
            _spawnTimeWas += Time.deltaTime;

            if (_spawnTimeWas > _spawnTime)
            {
                _spawnTimeWas = 0;

                _currentFruit = Instantiate(_fruitObject[_currentIndexFruit].FruitPrefab,
                    _spawnPointFruit.transform.position,
                    _fruitObject[_currentIndexFruit].FruitPrefab.transform.rotation)
                    .GetComponent<Fruit>();
       
                _currentFruit.Init(_basket);
                _currentFruit.transform.SetParent(transform, true);
                _currentFruit.GetComponent<FruitMovement>().SetParentPlant(this);
            }
        }
    }
    public void StopSpawn()
    {
        _isGrabbing = true;
    }

    public void StartSpawn()
    {
        _isGrabbing = false;
    }

    public void Init(Basket basket)
    {
        _basket = basket;
    }

    public void DeleteFruit()
    {
        _currentFruit = null;
    }

    public void ChangeCurrentFruit(int currentFruit)
    {
        _currentIndexFruit = currentFruit;
    }

    private void Modification()
    {
        _treeClass = _fruitObject[_currentIndexFruit];
    }

    public void GetSpawnPoint(SpawnPoint spawnPoint)
    {
        _spawnPoint = spawnPoint;
    }

    public void ModificationFruit()
    {
        if (_currentIndexFruit < _fruitObject.Count)
        {
            _currentIndexFruit++;
            Modification();
        }

        if (_currentFruit != null)
        {
            if (_currentFruit.TryGetComponent(out FruitMovement fruit) && _currentFruit != null)
            {
                fruit.Jump();
            }
        }
    }

    public void ImprovePlantModel()
    {
        if (_currentIndexPlant < _prefabPlant.Count - 1)
        {
            _currentIndexPlant++;
        }
        else
        {
            _currentIndexPlant = 0;
        }

        foreach (GameObject plantPrefab in _prefabPlant)
        {
            if (plantPrefab == _prefabPlant[_currentIndexPlant])
            {
                plantPrefab.SetActive(true);
            }
            else
            {
                plantPrefab.SetActive(false);
            }
        }
    }

    public void ChoosePlantModel(int modelPlant)
    {
        foreach (GameObject plantPrefab in _prefabPlant)
        {
            if (modelPlant < _emptyModel)
            {
                if (plantPrefab == _prefabPlant[modelPlant])
                {
                    plantPrefab.SetActive(true);
                }
                else
                {
                    plantPrefab.SetActive(false);
                }
            }
        }
    }

    public void UpSpeed(float newTime)
    {
        if (_spawnTime <= newTime)
        {
            newTime = 0;
        }
        _spawnTime -= newTime;
    }

    public void TakePlant()
    {
        _isGrabbing = true;
    }

    public void Release()
    {
        _isGrabbing = false;
    }

    public void PlayEffect()
    {
        _plantEffect.Play();
    }
}
