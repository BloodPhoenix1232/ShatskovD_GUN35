using DG.Tweening;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Fruit))]
[RequireComponent(typeof(BoxCollider))]
public class FruitMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _radiusTarget;
    [SerializeField] private Basket _basket;


    private Plant _plant;
    private Rigidbody _rb;
    private Fruit _fruit;
    private bool _isCut = true;
    private BoxCollider _collider;
    private float _jumpForce = 3f;
    private int _numJumps = 2;
    private float _duration = 1.2f;

    public static Action<Fruit> OnCut;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
        _fruit = GetComponent<Fruit>();
    }

    public void SetParentPlant(Plant plant)
    {
        _plant = plant;
    }

    public void Jump()
    {
        if(_isCut)
        {
            _plant.DeleteFruit();
            _rb.DOJump(_fruit.Basket.transform.position + new Vector3(Random.Range(-_radiusTarget, _radiusTarget), 0f, Random.Range(-_radiusTarget, _radiusTarget)),
            _jumpForce, _numJumps, _duration, false).OnComplete(() =>
            { _collider.enabled = true; _rb.useGravity = true; });
            _rb.isKinematic = false;
            gameObject.transform.parent = null;

            OnCut?.Invoke(_fruit);
            _isCut = false;
        }
    }
}