using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    [Header("Animation Settings")]
    [SerializeField] private Animator animator; // Reference to the Animator component
    [SerializeField] private float runThreshold = 0.1f; // Speed threshold to trigger run animation
    
    [Header("References")]
    [SerializeField] private CharacterMover characterMover; // Reference to the CharacterMover component
    
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
    }
    
    void LateUpdate()
    {
        UpdateAnimation();
    }

    
    private void UpdateAnimation()
    {
        if (animator == null) return;
        
        animator.SetBool(RUN_BOOLEAN, characterMover.MoveSpeed > runThreshold);
    }
  
}
