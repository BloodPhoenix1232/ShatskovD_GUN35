using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basket : MonoBehaviour
{
    [SerializeField] private int _basketSize;
    [SerializeField] private int _numberFruitsRemove;
    [SerializeField] private float _timeFruitRemoval;

    private List<Fruit> _fruits = new List<Fruit>();
    private Coroutine _deleteFruitsCoroutine;
    private bool _isDelete = true;

    private void OnEnable()
    {
        FruitMovement.OnCut += IncreaseFruit;
    }

    private void OnDisable()
    {
        FruitMovement.OnCut -= IncreaseFruit;
    }

    private void IncreaseFruit(Fruit newFruit)
    {
        _fruits.Add(newFruit);
        RemovingExcessFruit();
    }

    private void RemovingExcessFruit()
    {
        if (IsFullness() && _isDelete)
        {
            _deleteFruitsCoroutine = StartCoroutine(DeleteFruitsCoroutine());
        }
    }

    private IEnumerator DeleteFruitsCoroutine()
    {
        _isDelete = false;
        WaitForSeconds waitForSeconds = new WaitForSeconds(_timeFruitRemoval);

        yield return waitForSeconds;

        DeleteFruits();
        RemovingExcessFruit();
    }

    private bool IsFullness()
    {
        if (_fruits.Count >= _basketSize)
        {
            return true;
        }
        return false;
    }

    private void DeleteFruits()
    {
        Fruit fruit;

        for (int i = 0; i < _numberFruitsRemove; i++)
        {
            if (i < _fruits.Count)
            {
                fruit = _fruits[i];
                _fruits.RemoveAt(i);
                Destroy(fruit.gameObject);
                i--;
            }
        }

        UpdateListFruits();
    }

    private void UpdateListFruits()
    {
        List<Fruit> newFruits = new List<Fruit>();

        foreach (Fruit newFruit in _fruits)
        {
            if (newFruit != null)
            {
                newFruits.Add(newFruit);
            }
        }

        _fruits = newFruits;
        _isDelete = true;
    }
}
