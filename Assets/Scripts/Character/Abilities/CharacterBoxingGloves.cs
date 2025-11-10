using UnityEngine;
using System.Collections.Generic;

public class CharacterBoxingGloves : CharacterAbility
{
    [Header("Boxing Settings")]
    [SerializeField] private GameObject leftGlove;
    [SerializeField] private GameObject rightGlove;
    [SerializeField] private BoxCollider punchTrigger;
    
    [Header("Animation")]
    [SerializeField] private string punchAnimationTrigger = "Punch";
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem[] hitVFX;
    [SerializeField] private ParticleSystem[] punchVFX;
    
    [Header("Screen Shake")]
    [SerializeField] private bool enableScreenShake = true;
    [SerializeField] private ShakeIntensity punchShakeIntensity = ShakeIntensity.Medium;
    
    private Animator _animator;
    private HashSet<CharacterMover> _hitCharacters = new HashSet<CharacterMover>();

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
        _hitCharacters.Clear();
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
            if (_hitCharacters.Contains(otherCharacter))
            {
                return;
            }
            
            _hitCharacters.Add(otherCharacter);
            
            TriggerPunchAnimation();
            PunchCharacter(other.gameObject);
        }
    }
    
    private void PunchCharacter(GameObject target)
    {
        CharacterPunchAnimation punchAnimation = GetComponent<CharacterPunchAnimation>();
        if (punchAnimation != null)
        {
            punchAnimation.TriggerPunchAnimation();
        }
        
        PlayPunchVFX();
        PlayHitVFX(target.transform.position);
        
        SoundManager.PlaySound(SoundType.Scratch,0f, 1.2f);
        SoundManager.PlaySound(SoundType.Nope,0.6f, 1f);
        SoundManager.PlaySound(SoundType.Punch,0.2f, .3f);

        CharacterMover targetMover = target.GetComponent<CharacterMover>();
        if (enableScreenShake && targetMover != null && targetMover.IsPlayer)
        {
            CameraManager.TriggerShake(punchShakeIntensity);
        }
        
        CharacterDefeat defeat = target.GetComponent<CharacterDefeat>();
        if (defeat == null)
        {
            defeat = target.AddComponent<CharacterDefeat>();
        }
        defeat.TriggerDefeat();
    }

    private void PlayPunchVFX()
    {
        if (punchVFX == null) return;
        
        foreach (var fx in punchVFX)
        {
            if (fx != null)
            {
                fx.Play();
            }
        }
    }

    private void PlayHitVFX(Vector3 hitPosition)
    {
        if (hitVFX == null) return;
        
        foreach (var fx in hitVFX)
        {
            if (fx != null)
            {
                ParticleSystem vfx = Instantiate(fx, hitPosition, fx.transform.rotation);
                vfx.Play();
                Destroy(vfx.gameObject, vfx.main.duration + vfx.main.startLifetime.constantMax);
            }
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

