using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI currencyText;
    
    [Header("Text Settings")]
    [SerializeField] private string currencyFormat = "{0}";
    
    private CharacterCurrency _playerCurrency;

    private void Awake()
    {
        if (currencyText == null)
        {
            currencyText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    private void Start()
    {
        FindPlayerCurrency();
        
        if (_playerCurrency != null)
        {
            _playerCurrency.OnCurrencyChanged.AddListener(OnCurrencyChanged);
            UpdateCurrencyDisplay(_playerCurrency.CurrentCurrency);
        }
    }

    private void OnDestroy()
    {
        if (_playerCurrency != null)
        {
            _playerCurrency.OnCurrencyChanged.RemoveListener(OnCurrencyChanged);
        }
    }

    private void FindPlayerCurrency()
    {
        CharacterCurrency[] allCurrencies = FindObjectsOfType<CharacterCurrency>();
        
        foreach (var currency in allCurrencies)
        {
            if (currency.IsPlayer)
            {
                _playerCurrency = currency;
                break;
            }
        }
    }

    private void OnCurrencyChanged(int newAmount)
    {
        UpdateCurrencyDisplay(newAmount);
    }

    private void UpdateCurrencyDisplay(int amount)
    {
        if (currencyText != null)
        {
            currencyText.text = string.Format(currencyFormat, amount);
        }
    }
}






