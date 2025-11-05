using UnityEngine;

public class SpeedBoostPortal : Portal
{
    [Header("Speed Boost Settings")]
    [SerializeField] private float speedMultiplier = 2f;
    [SerializeField] private float duration = 3f;

    protected override void ActivatePortal(CharacterAbilityHandler abilityHandler)
    {
        abilityHandler.ActivateSpeedBoost(speedMultiplier, duration);
    }
}

