using UnityEngine;

public class CharacterSpeedBoost : CharacterAbility
{
    [Header("VFX References")]
    [SerializeField] private ParticleSystem speedTrailFX;
    [SerializeField] private ParticleSystem speedBoostFX;
    
    [Header("Speed Settings")]
    [SerializeField] private float speedMultiplier = 2f;
    
    private float _originalSpeed;

    public void Activate(float multiplier, float duration)
    {
        speedMultiplier = multiplier;
        base.Activate(duration);
    }

    protected override void OnAbilityStart()
    {
        if (_characterMover != null)
        {
            _originalSpeed = _characterMover.GetMoveSpeed();
            _characterMover.SetMoveSpeed(_originalSpeed * speedMultiplier);
        }
        
        PlayVFX();
    }

    protected override void OnAbilityEnd()
    {
        if (_characterMover != null)
        {
            _characterMover.SetMoveSpeed(_originalSpeed);
        }
        ;
        StopVFX();
    }

    private void PlayVFX()
    {
        if (speedTrailFX != null)
        {
            speedTrailFX.Play();
        }
        
        if (speedBoostFX != null)
        {
            speedBoostFX.Play();
        }
    }

    private void StopVFX()
    {
        if (speedTrailFX != null)
        {
            speedTrailFX.Stop();
        }
        
        if (speedBoostFX != null)
        {
            speedBoostFX.Stop();
        }
    }
}

