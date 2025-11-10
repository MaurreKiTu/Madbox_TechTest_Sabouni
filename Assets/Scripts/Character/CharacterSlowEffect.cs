using System.Collections;
using UnityEngine;

public class CharacterSlowEffect : MonoBehaviour
{
    [Header("Slow Settings")]
    [SerializeField] private float slowSpeedMultiplier = 0.3f;
    [SerializeField] private float defaultSlowDuration = 2f;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem[] slowVFX;
    
    private CharacterMover _characterMover;
    private Coroutine _slowCoroutine;
    private float _originalSpeed;
    private bool _isSlowed = false;

    public bool IsSlowed => _isSlowed;

    private void Awake()
    {
        _characterMover = GetComponent<CharacterMover>();
    }

    public void ApplySlow(float duration = -1f)
    {
        if (_characterMover == null) return;
        
        if (_isSlowed) return;
        
        if (duration < 0)
        {
            duration = defaultSlowDuration;
        }
        
        _slowCoroutine = StartCoroutine(SlowCoroutine(duration));
    }

    private IEnumerator SlowCoroutine(float duration)
    {
        _isSlowed = true;
        
        _originalSpeed = _characterMover.GetMoveSpeed();
        _characterMover.SetMoveSpeed(_originalSpeed * slowSpeedMultiplier);
        
        if (_characterMover.IsPlayer)
        {
            SoundManager.PlaySound(SoundType.Slowed);
        }
        
        PlaySlowVFX();
        
        yield return new WaitForSeconds(duration);
        
        _characterMover.SetMoveSpeed(_originalSpeed);
        
        StopSlowVFX();
        
        _isSlowed = false;
        _slowCoroutine = null;
    }

    private void PlaySlowVFX()
    {
        if (slowVFX == null) return;
        
        foreach (var fx in slowVFX)
        {
            if (fx != null)
            {
                fx.Play();
            }
        }
    }

    private void StopSlowVFX()
    {
        if (slowVFX == null) return;
        
        foreach (var fx in slowVFX)
        {
            if (fx != null)
            {
                fx.Stop();
            }
        }
    }

    public void CancelSlow()
    {
        if (_slowCoroutine != null)
        {
            StopCoroutine(_slowCoroutine);
            _characterMover.SetMoveSpeed(_originalSpeed);
            StopSlowVFX();
            _isSlowed = false;
            _slowCoroutine = null;
        }
    }
}

