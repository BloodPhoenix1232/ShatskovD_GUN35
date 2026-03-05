using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    private bool _isBusy;
    private Plant _plant;

    public bool IsBusy => _isBusy;

    public void GetPlant(Plant plant)
    {
        _plant = plant;
        GetStatus();
    }

    private void GetStatus()
    {
        if (_plant != null)
        {
            _isBusy = true;
        }
        else
        {
            _isBusy = false;
        }
    }

    public void DeletePlant()
    {
        _plant = null;
        GetStatus();
    }    
}
