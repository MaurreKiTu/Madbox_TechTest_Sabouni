using UnityEngine;

public class BoxingPortal : Portal
{
    [Header("Boxing Settings")]
    [SerializeField] private float duration = 5f;

    protected override void ActivatePortal(CharacterAbilityHandler abilityHandler)
    {
        abilityHandler.ActivateBoxingGloves(duration);
    }
}

