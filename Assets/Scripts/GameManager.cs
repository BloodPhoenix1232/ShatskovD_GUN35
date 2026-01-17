using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PinSpawn pinSpawner;

    private int _throwCount = 0;
    private int _pinsDownThisThrow = 0;
    private int _pinsDownLastThrow = 0;

    private int _bonusThrows = 0;
    private bool _isReady = true;

    void Awake()
    {
        Instance = this;
    }

    public void StartThrow()
    {
        _pinsDownThisThrow = 0; 
    }

    public void PinDown()
    {
        _pinsDownThisThrow++;

        Score.Instance.PinDown(null);

        if (_bonusThrows > 0)
        {
            Score.Instance.PinDown(null);
        }
    }

    public void EndThrow()
    {
        if (_bonusThrows > 0) _bonusThrows--;
        _throwCount++;

        StartCoroutine(StrikeChecker());

        if (_throwCount == 1)
        {
            StartCoroutine(SetReadyDelay());
        }

        if (_throwCount == 2)
        {
            StartCoroutine(SpareChecker());
        }
    }

    public bool ReadyToLaunch()
    {
        if (_isReady) 
        {
            _isReady = false;
            return true; 
        }
        else { return false; }
    }

    private IEnumerator SpawnPinsDelayed()
    {
        yield return new WaitForSeconds(3f);

        pinSpawner.ClearPins();

        pinSpawner.SpawnPins();
        _isReady = true;
    }

    private IEnumerator SetReadyDelay()
    {
        yield return new WaitForSeconds(10f);

        _isReady = true;
    }

    private IEnumerator StrikeChecker()
    {
        yield return new WaitForSeconds(6f);
        _pinsDownLastThrow = _pinsDownThisThrow;

        if (_throwCount == 1 && _pinsDownThisThrow == 10)
        {
            Debug.Log("Strike");
            _throwCount = 0;
            _bonusThrows = 2;
            StartCoroutine(SpawnPinsDelayed());
        }
    }

    private IEnumerator SpareChecker()
    {
        yield return new WaitForSeconds(4f);

        if (_throwCount == 2 && _pinsDownThisThrow + _pinsDownLastThrow == 10)
        {
            Debug.Log("Spare");
            _throwCount = 0;
            _bonusThrows = 1;
            StartCoroutine(SpawnPinsDelayed());
        }
        else
        {
            _throwCount = 0;
            StartCoroutine(SpawnPinsDelayed());
        }
    }
}
