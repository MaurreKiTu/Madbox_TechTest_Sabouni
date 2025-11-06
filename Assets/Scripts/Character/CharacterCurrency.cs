using UnityEngine;
using UnityEngine.Events;

public class CharacterCurrency : MonoBehaviour
{
    [Header("Currency Settings")]
    [SerializeField] private int startingCurrency = 0;
    
    private int _currentCurrency;
    private CharacterMover _characterMover;
    
    public UnityEvent<int> OnCurrencyChanged = new UnityEvent<int>();
    
    public int CurrentCurrency => _currentCurrency;
    public bool IsPlayer => _characterMover != null && _characterMover.IsPlayer;

    private void Awake()
    {
        _characterMover = GetComponent<CharacterMover>();
        _currentCurrency = startingCurrency;
    }

    public void AddCurrency(int amount)
    {
        if (amount <= 0) return;
        
        _currentCurrency += amount;
        OnCurrencyChanged?.Invoke(_currentCurrency);
    }

    public bool SpendCurrency(int amount)
    {
        if (amount <= 0 || _currentCurrency < amount) return false;
        
        _currentCurrency -= amount;
        OnCurrencyChanged?.Invoke(_currentCurrency);
        return true;
    }

    public bool HasEnoughCurrency(int amount)
    {
        return _currentCurrency >= amount;
    }
}

