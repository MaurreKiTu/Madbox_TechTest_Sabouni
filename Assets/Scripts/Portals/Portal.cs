using UnityEngine;

public abstract class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private bool deactivateAfterUse = false;
    [SerializeField] private int cost = 0;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem activationFX;
    [SerializeField] private ParticleSystem insufficientFundsFX;
    
    [Header("Cost Indicator")]
    [SerializeField] private GameObject costIndicatorPrefab;
    [SerializeField] private Canvas uiCanvas;
    
    private bool _hasBeenUsed;

    public int Cost => cost;

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
                    OnInsufficientFunds();
                    return;
                }
                
                currency.SpendCurrency(cost);
                
                if (currency.IsPlayer)
                {
                    ShowCostIndicator(other.transform.position);
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
    }

    private void ShowCostIndicator(Vector3 playerPosition)
    {
        if (costIndicatorPrefab == null) return;
        
        if (uiCanvas == null)
        {
            uiCanvas = FindObjectOfType<Canvas>();
        }
        
        if (uiCanvas == null) return;
        
        GameObject indicatorObj = Instantiate(costIndicatorPrefab, uiCanvas.transform);
        PortalCostIndicatorUI indicator = indicatorObj.GetComponent<PortalCostIndicatorUI>();
        
        if (indicator != null)
        {
            indicator.ShowCost(playerPosition, cost);
        }
    }

    public void ResetPortal()
    {
        _hasBeenUsed = false;
    }
}

