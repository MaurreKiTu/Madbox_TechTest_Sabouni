using UnityEngine;

public abstract class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private bool deactivateAfterUse = false;
    [SerializeField] private int cost = 0;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem activationFX;
    [SerializeField] private ParticleSystem insufficientFundsFX;
    
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

    public void ResetPortal()
    {
        _hasBeenUsed = false;
    }
}

