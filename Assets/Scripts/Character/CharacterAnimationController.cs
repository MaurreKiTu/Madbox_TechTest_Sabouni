using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator animator;
    [SerializeField] private float runThreshold = 0.1f;
    
    [Header("References")]
    [SerializeField] private CharacterMover characterMover;
    
    [Header("Animation Speed Settings")]
    [SerializeField] private bool adaptAnimationSpeed = true;
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float minAnimationSpeed = 0.3f;
    [SerializeField] private float maxAnimationSpeed = 2f;
    
    private const string RUN_BOOLEAN = "Run";
    
    void Start()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        
        if (characterMover == null)
        {
            characterMover = GetComponent<CharacterMover>();
        }
        
        if (animator != null)
        {
            animator.SetBool(RUN_BOOLEAN, false);
        }
        
        if (characterMover != null && normalSpeed <= 0)
        {
            normalSpeed = characterMover.GetMoveSpeed();
        }
    }
    
    void LateUpdate()
    {
        UpdateAnimation();
    }

    
    private void UpdateAnimation()
    {
        if (animator == null || characterMover == null) return;
        
        float currentSpeed = characterMover.MoveSpeed;
        
        animator.SetBool(RUN_BOOLEAN, currentSpeed > runThreshold);
        
        if (adaptAnimationSpeed && normalSpeed > 0)
        {
            float speedRatio = currentSpeed / normalSpeed;
            float animSpeed = Mathf.Clamp(speedRatio, minAnimationSpeed, maxAnimationSpeed);
            animator.speed = animSpeed;
        }
    }
  
}
