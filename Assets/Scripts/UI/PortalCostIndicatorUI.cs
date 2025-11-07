using System.Collections;
using TMPro;
using UnityEngine;

public class PortalCostIndicatorUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TextMeshProUGUI costText;
    
    [Header("Animation Settings")]
    [SerializeField] private float displayDuration = 1f;
    [SerializeField] private float floatDistance = 50f;
    [SerializeField] private float heightOffset = 2f;
    [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);
    
    [Header("Text Settings")]
    [SerializeField] private string costFormat = "-${0}";
    [SerializeField] private Color textColor = Color.red;
    
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Vector3 _startPosition;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        _canvasGroup = GetComponent<CanvasGroup>();
        if (_canvasGroup == null)
        {
            _canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        if (costText == null)
        {
            costText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void ShowCost(Vector3 worldPosition, int cost)
    {
        if (costText != null)
        {
            costText.text = string.Format(costFormat, cost);
            costText.color = textColor;
        }
        
        Vector3 offsetPosition = worldPosition + Vector3.up * heightOffset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(offsetPosition);
        _rectTransform.position = screenPos;
        _startPosition = _rectTransform.anchoredPosition;
        
        StartCoroutine(AnimateCoroutine());
    }

    private IEnumerator AnimateCoroutine()
    {
        float elapsed = 0f;
        
        while (elapsed < displayDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / displayDuration;
            
            float fadeValue = fadeCurve.Evaluate(t);
            _canvasGroup.alpha = fadeValue;
            
            Vector3 offset = Vector3.up * (floatDistance * t);
            _rectTransform.anchoredPosition = _startPosition + offset;
            
            yield return null;
        }
        
        Destroy(gameObject);
    }
}

