using UnityEngine;

public class SlowObstacle : MonoBehaviour
{
    [Header("Slow Settings")]
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private bool affectPlayerOnly = false;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem hitFX;
    
    [Header("Screen Shake")]
    [SerializeField] private bool enableScreenShake = true;
    [SerializeField] private ShakeIntensity shakeIntensity = ShakeIntensity.Light;
    
    private void OnTriggerEnter(Collider other)
    {
        CharacterSlowEffect slowEffect = other.GetComponent<CharacterSlowEffect>();
        
        if (slowEffect != null)
        {
            CharacterMover mover = other.GetComponent<CharacterMover>();
            bool isPlayer = mover != null && mover.IsPlayer;
            
            if (affectPlayerOnly && !isPlayer)
            {
                return;
            }
            
            if (enableScreenShake && isPlayer)
            {
                CameraManager.TriggerShake(shakeIntensity);
            }
            
            slowEffect.ApplySlow(slowDuration);
            PlayHitFX();
        }
    }

    private void PlayHitFX()
    {
        if (hitFX != null)
        {
            hitFX.Play();
        }
    }
}

