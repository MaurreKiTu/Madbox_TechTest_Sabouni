using TMPro;
using UnityEngine;
using DG.Tweening;

public class CurrencyUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI currencyText;

    [Header("Text Settings")]
    [SerializeField] private string currencyFormat = "{0}";

    [Header("Hide Animation")]
    [SerializeField] private float hideAnimationDuration = 0.5f;
    [SerializeField] private Ease hideAnimationEase = Ease.InBack;

    private CharacterCurrency _playerCurrency;
    private RectTransform _rectTransform;
    private Vector2 _originalPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPosition = _rectTransform.anchoredPosition;
        
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
        
        if (_rectTransform != null)
        {
            _rectTransform.DOKill();
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

    public void HideToTop()
    {
        if (_rectTransform == null) return;
        
        float screenHeight = Screen.height;
        float targetY = screenHeight + _rectTransform.rect.height;
        
        _rectTransform.DOAnchorPosY(targetY, hideAnimationDuration)
            .SetEase(hideAnimationEase)
            .SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }
}






