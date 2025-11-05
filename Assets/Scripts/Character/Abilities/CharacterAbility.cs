using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CharacterAbilityHandler))]
public abstract class CharacterAbility : MonoBehaviour
{
    protected bool _isActive;
    protected Coroutine _abilityCoroutine;
    protected CharacterMover _characterMover;

    public bool IsActive => _isActive;

    protected virtual void Awake()
    {
        _characterMover = GetComponent<CharacterMover>();
    }

    public void Activate(float duration)
    {
        if (_isActive)
        {
            if (_abilityCoroutine != null)
            {
                StopCoroutine(_abilityCoroutine);
            }
        }
        
        _abilityCoroutine = StartCoroutine(AbilityCoroutine(duration));
    }

    private IEnumerator AbilityCoroutine(float duration)
    {
        _isActive = true;
        
        OnAbilityStart();
        
        yield return new WaitForSeconds(duration);
        
        OnAbilityEnd();
        
        _isActive = false;
        _abilityCoroutine = null;
    }

    protected abstract void OnAbilityStart();
    protected abstract void OnAbilityEnd();

    protected virtual void OnDisable()
    {
        if (_isActive)
        {
            OnAbilityEnd();
        }
    }
}

