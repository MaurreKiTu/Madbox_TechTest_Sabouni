using System.Collections;
using UnityEngine;

public class CharacterPunchAnimation : MonoBehaviour
{
    [Header("Spin Settings")]
    [SerializeField] private float spinDuration = 0.3f;
    [SerializeField] private float spinRotations = 1f;
    [SerializeField] private AnimationCurve spinCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("Jump Settings")]
    [SerializeField] private bool addJump = true;
    [SerializeField] private float jumpHeight = 0.5f;
    
    [Header("References")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private Animator animatorReference;

    private CharacterMover _characterMover;
    private bool _isAnimating;

    private void Awake()
    {
        _characterMover = GetComponent<CharacterMover>();
        
        if (visualTransform == null)
        {
            if (animatorReference != null)
            {
                visualTransform = animatorReference.transform;
            }
            else
            {
                visualTransform = transform;
            }
        }
    }

    public void TriggerPunchAnimation()
    {
        if (_isAnimating) return;
        
        StartCoroutine(PunchAnimationCoroutine());
    }

    private IEnumerator PunchAnimationCoroutine()
    {
        _isAnimating = true;
        
        bool wasEnabled = false;
        if (_characterMover != null)
        {
            wasEnabled = _characterMover.enabled;
            _characterMover.enabled = false;
        }
        
        bool animatorWasEnabled = false;
        if (animatorReference != null)
        {
            animatorWasEnabled = animatorReference.enabled;
            animatorReference.enabled = false;
        }
        
        Vector3 startRotation = visualTransform.localEulerAngles;
        float totalRotation = 360f * spinRotations;
        
        Vector3 startPosition = transform.position;
        float elapsed = 0f;

        while (elapsed < spinDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / spinDuration;
            float curveValue = spinCurve.Evaluate(t);
            
            float currentRotation = totalRotation * curveValue;
            visualTransform.localEulerAngles = startRotation + new Vector3(0, 0, currentRotation);
            
            if (addJump)
            {
                float jumpOffset = Mathf.Sin(t * Mathf.PI) * jumpHeight;
                transform.position = startPosition + Vector3.up * jumpOffset;
            }
            
            yield return null;
        }

        visualTransform.localEulerAngles = startRotation + new Vector3(0, 0, totalRotation);
        transform.position = startPosition;
        
        if (animatorReference != null && animatorWasEnabled)
        {
            animatorReference.enabled = true;
        }
        
        if (_characterMover != null && wasEnabled)
        {
            _characterMover.enabled = true;
        }
        
        _isAnimating = false;
    }
}

