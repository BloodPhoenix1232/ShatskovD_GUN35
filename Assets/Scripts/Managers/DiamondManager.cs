using UnityEngine;

public class DiamondManager : MonoBehaviour
{
    private int _diamonds = 0;

    public int Diamonds => _diamonds;

    public delegate void OnDiamondsChanged(int diamonds);
    public event OnDiamondsChanged DiamondsChanged;

    private static DiamondManager _instance;
    public static DiamondManager Instance => _instance;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadDiamonds();
    }

    public void AddDiamonds(int amount)
    {
        _diamonds += amount;
        DiamondsChanged?.Invoke(_diamonds);
        SaveDiamonds();
    }

    private void SaveDiamonds()
    {
        SaveSystem.Instance?.SetDiamonds(_diamonds);
    }

    private void LoadDiamonds()
    {
        SaveData data = SaveSystem.Instance?.GetData();
        if (data != null)
        {
            _diamonds = data.diamonds;
            DiamondsChanged?.Invoke(_diamonds);
        }
    }
}