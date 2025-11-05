using UnityEngine;

public class CharacterAbilityHandler : MonoBehaviour
{
    private CharacterMover _characterMover;
    
    private void Awake()
    {
        _characterMover = GetComponent<CharacterMover>();
    }

    public void ActivateSpeedBoost(float speedMultiplier, float duration)
    {
        CharacterSpeedBoost speedBoost = GetComponent<CharacterSpeedBoost>();
        if (speedBoost == null)
        {
            speedBoost = gameObject.AddComponent<CharacterSpeedBoost>();
        }
        speedBoost.Activate(speedMultiplier, duration);
    }

    public void ActivateBoxingGloves(float duration)
    {
        CharacterBoxingGloves boxingGloves = GetComponent<CharacterBoxingGloves>();
        if (boxingGloves == null)
        {
            boxingGloves = gameObject.AddComponent<CharacterBoxingGloves>();
        }
        boxingGloves.Activate(duration);
    }

    public CharacterMover GetCharacterMover()
    {
        return _characterMover;
    }
}

