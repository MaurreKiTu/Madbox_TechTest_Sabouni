using UnityEngine;

public abstract class Portal : MonoBehaviour
{
    [Header("Portal Settings")]
    [SerializeField] private bool deactivateAfterUse = false;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem activationFX;
    
    private bool _hasBeenUsed;

    private void OnTriggerEnter(Collider other)
    {
        if (deactivateAfterUse && _hasBeenUsed) return;
        
        CharacterAbilityHandler abilityHandler = other.GetComponent<CharacterAbilityHandler>();
        if (abilityHandler != null)
        {

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

    public void ResetPortal()
    {
        _hasBeenUsed = false;
    }
}

