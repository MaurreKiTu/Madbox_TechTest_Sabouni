using UnityEngine;

public class CharacterBoxingGloves : CharacterAbility
{
    [Header("Boxing Settings")]
    [SerializeField] private GameObject leftGlove;
    [SerializeField] private GameObject rightGlove;
    [SerializeField] private BoxCollider punchTrigger;
    
    [Header("Animation")]
    [SerializeField] private string punchAnimationTrigger = "Punch";
    
    [Header("Ejection Settings")]
    [SerializeField] private float ejectionForce = 20f;
    [SerializeField] private float upwardForce = 10f;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem hitVFX;
    
    private Animator _animator;

    protected override void Awake()
    {
        base.Awake();
        
        _animator = GetComponent<Animator>();
        
        if (leftGlove != null)
        {
            leftGlove.SetActive(false);
        }
        
        if (rightGlove != null)
        {
            rightGlove.SetActive(false);
        }
        
        if (punchTrigger != null)
        {
            DisablePunchTrigger();
        }
    }

    protected override void OnAbilityStart()
    {
        ShowGloves();
        EnablePunchTrigger();
    }

    protected override void OnAbilityEnd()
    {
        HideGloves();
        DisablePunchTrigger();
    }

    private void ShowGloves()
    {
        if (leftGlove != null)
        {
            leftGlove.SetActive(true);
        }
        
        if (rightGlove != null)
        {
            rightGlove.SetActive(true);
        }
    }

    private void HideGloves()
    {
        if (leftGlove != null)
        {
            leftGlove.SetActive(false);
        }
        
        if (rightGlove != null)
        {
            rightGlove.SetActive(false);
        }
    }

    private void EnablePunchTrigger()
    {
        if (punchTrigger != null)
        {
            punchTrigger.enabled = true;
        }
    }

    private void DisablePunchTrigger()
    {
        if (punchTrigger != null)
        {
            punchTrigger.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_isActive) return;
        
        CharacterMover otherCharacter = other.GetComponent<CharacterMover>();
        if (otherCharacter != null && otherCharacter != _characterMover)
        {
            TriggerPunchAnimation();
            EjectCharacter(other.gameObject);
        }
    }
    
    private void EjectCharacter(GameObject target)
    {
        CharacterPunchAnimation punchAnimation = GetComponent<CharacterPunchAnimation>();
        if (punchAnimation != null)
        {
            punchAnimation.TriggerPunchAnimation();
        }
        
        PlayHitVFX(target.transform.position);
        
        CharacterEjection ejection = target.GetComponent<CharacterEjection>();
        if (ejection == null)
        {
            ejection = target.AddComponent<CharacterEjection>();
        }
        
        Vector3 ejectionDirection = (target.transform.position - transform.position).normalized;
        ejection.Eject(ejectionDirection, ejectionForce, upwardForce);
    }

    private void PlayHitVFX(Vector3 hitPosition)
    {
        if (hitVFX != null)
        {
            ParticleSystem vfx = Instantiate(hitVFX, hitPosition, Quaternion.identity);
            vfx.Play();
            Destroy(vfx.gameObject, vfx.main.duration + vfx.main.startLifetime.constantMax);
        }
    }

    private void TriggerPunchAnimation()
    {
        if (_animator != null)
        {
            _animator.SetTrigger(punchAnimationTrigger);
        }
    }
}

