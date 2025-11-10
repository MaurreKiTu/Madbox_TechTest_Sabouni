using UnityEngine;
using System.Collections;

public abstract class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private bool deactivateAfterUse = false;
    [SerializeField] private int cost = 0;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem activationFX;
    [SerializeField] private ParticleSystem insufficientFundsFX;
    
    [Header("UI References")]
    [SerializeField] private PortalCostUI costUI;
    [SerializeField] private GameObject paymentFeedbackPrefab;
    [SerializeField] private Canvas uiCanvas;
    
    [Header("Shake Settings")]
    [SerializeField] private float shakeIntensity = 0.3f;
    [SerializeField] private float shakeDuration = 0.5f;
    [SerializeField] private int shakeVibrations = 10;
    
    private bool _hasBeenUsed;
    private Vector3 _originalPosition;
    private Coroutine _shakeCoroutine;

    public int Cost => cost;

    private void Awake()
    {
        _originalPosition = transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (deactivateAfterUse && _hasBeenUsed) return;
        
        CharacterAbilityHandler abilityHandler = other.GetComponent<CharacterAbilityHandler>();
        if (abilityHandler != null)
        {
            CharacterCurrency currency = other.GetComponent<CharacterCurrency>();
            
            if (cost > 0)
            {
                if (currency == null || !currency.HasEnoughCurrency(cost))
                {
                    if (currency != null && currency.IsPlayer)
                    {
                        OnInsufficientFunds();
                        SoundManager.PlaySound(SoundType.PortalNotEnough);

                    }
                    return;
                }
                
                currency.SpendCurrency(cost);
                
                if (currency.IsPlayer)
                {
                    ShowCostIndicator(other.transform.position);
                    SoundManager.PlaySound(SoundType.PortalBuy);
                    if (costUI != null)
                    {
                        costUI.FlashGreen();
                    }
                }
            }
            
            ActivatePortal(abilityHandler);
            PlayActivationFX();
            
            if (deactivateAfterUse)
            {
                _hasBeenUsed = true;
            }
        }
    }

    protected abstract void ActivatePortal(CharacterAbilityHandler abilityHandler);

    private void PlayActivationFX()
    {
        if (activationFX != null)
        {
            activationFX.Play();
        }
    }

    protected virtual void OnInsufficientFunds()
    {
        if (insufficientFundsFX != null)
        {
            insufficientFundsFX.Play();
        }
        
        if (_shakeCoroutine != null)
        {
            StopCoroutine(_shakeCoroutine);
            transform.localPosition = _originalPosition;
        }
        _shakeCoroutine = StartCoroutine(ShakePortal());
        
        if (costUI != null)
        {
            costUI.FlashRed();
        }
    }

    private void ShowCostIndicator(Vector3 playerPosition)
    {
        if (paymentFeedbackPrefab == null) return;
        
        if (uiCanvas == null)
        {
            uiCanvas = FindObjectOfType<Canvas>();
        }
        
        if (uiCanvas == null) return;
        
        GameObject feedbackObj = Instantiate(paymentFeedbackPrefab, uiCanvas.transform);
        PaymentFeedbackUI feedback = feedbackObj.GetComponent<PaymentFeedbackUI>();
        
        if (feedback != null)
        {
            feedback.ShowCost(playerPosition, cost);
        }
    }
    
    private IEnumerator ShakePortal()
    {
        float elapsed = 0f;
        
        while (elapsed < shakeDuration)
        {
            float progress = elapsed / shakeDuration;
            float damping = 1f - progress;
            
            float offsetX = Mathf.Sin(elapsed * shakeVibrations * Mathf.PI * 2) * shakeIntensity * damping;
            transform.localPosition = _originalPosition + new Vector3(offsetX, 0, 0);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        transform.localPosition = _originalPosition;
        _shakeCoroutine = null;
    }

    public void ResetPortal()
    {
        _hasBeenUsed = false;
    }
}

