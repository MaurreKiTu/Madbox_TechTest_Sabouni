using UnityEngine;

public class CharacterAbilityHandler : MonoBehaviour
{

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

}

