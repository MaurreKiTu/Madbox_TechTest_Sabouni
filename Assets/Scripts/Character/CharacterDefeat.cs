using UnityEngine;
using System.Collections;

public class CharacterDefeat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform visualTransform;
    
    [Header("Animation Settings")]
    private string loseAnimationTrigger = "Lose";
    
    [Header("Camera Rotation")]
    [SerializeField] private float targetYRotation = 310f;
    
    [Header("Camera Transition")]
    [SerializeField] private float uiDelayAfterCameraSwitch = 1f;
    
    [Header("VFX")]
    [SerializeField] private ParticleSystem[] loseVFX;
    
    [Header("Music Transition")]
    [SerializeField] private float raceMusicFadeOutDuration = 0.3f;
    [SerializeField] private float gameOverMusicStartTime = 0f;
    [SerializeField] private float gameOverMusicFadeInDuration = 2.5f;
    [SerializeField] private float delayBeforeGameOverMusic = 0.5f;
    
    private CharacterMover _characterMover;
    private bool _isDefeated;

    private void Awake()
    {
        _characterMover = GetComponent<CharacterMover>();
        
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (visualTransform == null && transform.childCount > 0)
        {
            visualTransform = transform.GetChild(0);
        }
    }

    public void TriggerDefeat()
    {
        if (_isDefeated) return;
        
        _isDefeated = true;
        
        CameraManager cameraManager = FindObjectOfType<CameraManager>();
        if (cameraManager != null)
        {
            cameraManager.SwitchCamera(CameraType.Ending);
        }
        
        HandleMusicTransition();
        
        StartCoroutine(ShowGameOverAfterDelay());
        
        StopAllAbilities();
        
        if (_characterMover != null)
        {
            _characterMover.enabled = false;
        }
        
        RotateToCamera();
        PlayLoseVFX();
        PlayLoseAnimation();
    }

    private IEnumerator ShowGameOverAfterDelay()
    {
        yield return new WaitForSeconds(uiDelayAfterCameraSwitch);
        
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.OnPlayerDefeated.Invoke();
        }
    }

    private void HandleMusicTransition()
    {
        SoundManager.FadeOutMusic(raceMusicFadeOutDuration, true);
        StartCoroutine(PlayGameOverMusicDelayed());
    }

    private IEnumerator PlayGameOverMusicDelayed()
    {
        yield return new WaitForSeconds(raceMusicFadeOutDuration + delayBeforeGameOverMusic);
        SoundManager.PlayMusic(MusicType.GameOver, gameOverMusicStartTime, gameOverMusicFadeInDuration);
    }

    private void RotateToCamera()
    {
        if (visualTransform != null)
        {
            visualTransform.localRotation = Quaternion.Euler(0, targetYRotation, 0);
        }
    }

    private void PlayLoseVFX()
    {
        if (loseVFX == null) return;
        
        foreach (var fx in loseVFX)
        {
            if (fx != null)
            {
                fx.Play();
            }
        }
    }

    private void PlayLoseAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger(loseAnimationTrigger);
        }
    }

    private void StopAllAbilities()
    {
        CharacterAbility[] abilities = GetComponents<CharacterAbility>();
        foreach (var ability in abilities)
        {
            ability.enabled = false;
        }
    }

    public bool IsDefeated => _isDefeated;
}

