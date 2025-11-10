using UnityEngine;

public class SlowObstacle : MonoBehaviour
{
    [Header("Slow Settings")]
    [SerializeField] private float slowDuration = 2f;
    [SerializeField] private bool affectPlayerOnly = false;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem hitFX;
    
    private void OnTriggerEnter(Collider other)
    {
        CharacterSlowEffect slowEffect = other.GetComponent<CharacterSlowEffect>();
        
        if (slowEffect != null)
        {
            if (affectPlayerOnly)
            {
                CharacterMover mover = other.GetComponent<CharacterMover>();
                if (mover == null || !mover.IsPlayer)
                {
                    return;
                }
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

