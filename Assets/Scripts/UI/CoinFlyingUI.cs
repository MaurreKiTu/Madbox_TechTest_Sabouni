using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinFlyingUI : MonoBehaviour
{
    [Header("References")]
    private Image _coinImage;
    [SerializeField] private TextMeshProUGUI amountText;
    
    [Header("Animation Settings")]
    [SerializeField] private AnimationCurve movementCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float delayBeforeFlying = 0.5f;
    [SerializeField] private float duration = 0.8f;
    
    [Header("Text Settings")]
    [SerializeField] private string amountFormat = "+${0}";
    
    private RectTransform _rectTransform;
    private Action _onComplete;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        if (_coinImage == null)
        {
            _coinImage = GetComponent<Image>();
        }
        
        if (amountText == null)
        {
            amountText = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void FlyToTarget(Vector3 startWorldPos, Vector3 targetScreenPos, int amount, Action onComplete = null)
    {
        _onComplete = onComplete;
        
        if (amountText != null)
        {
            amountText.text = string.Format(amountFormat, amount);
        }
        
        Vector3 startScreenPos = Camera.main.WorldToScreenPoint(startWorldPos);
        _rectTransform.position = startScreenPos;
        
        StartCoroutine(FlyCoroutine(targetScreenPos));
    }

    private IEnumerator FlyCoroutine(Vector3 targetPos)
    {
        yield return new WaitForSeconds(delayBeforeFlying);
        
        Vector3 startPos = _rectTransform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curveValue = movementCurve.Evaluate(t);
            
            _rectTransform.position = Vector3.Lerp(startPos, targetPos, curveValue);
            
            yield return null;
        }

        _rectTransform.position = targetPos;
        _onComplete?.Invoke();
        Destroy(gameObject);
    }
}

