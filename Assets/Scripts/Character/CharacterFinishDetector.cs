using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class CharacterFinishDetector : MonoBehaviour
{
    [Header("Finish Detection")]
    [SerializeField] private string finishTag = "FinishLane";
    [SerializeField] private bool stopOnFinish = true;
    [SerializeField] private bool disableOnFinish = true;
    
    [Header("Deceleration Settings")]
    [SerializeField] private bool useDeceleration = true;
    [SerializeField] private float decelerationDuration = 2f;
    
    
    [Header("References")]
    [SerializeField] private CharacterMover characterMover;
    
    private bool hasFinished = false;
    private bool wasMoving = false;
    
    public UnityAction<CharacterFinishDetector> onFinishReached = delegate {};
    
    public bool HasFinished => hasFinished;

    public CharacterMover CharacterMover => characterMover;
    
    void Awake()
    {
        // Auto-find CharacterMover if not assigned
        if (characterMover == null)
        {
            characterMover = GetComponent<CharacterMover>();
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (hasFinished) return; // Already finished
        
        if (other.CompareTag(finishTag))
        {
            OnFinishReached();
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (hasFinished) return; // Already finished
        
        if (collision.gameObject.CompareTag(finishTag))
        {
            OnFinishReached();
        }
    }
    
    private void OnFinishReached()
    {
        if (hasFinished) return; // Prevent multiple triggers
        
        hasFinished = true;
        Debug.Log($"{gameObject.name} has reached the finish line!");
        
        // Trigger finish event
        onFinishReached.Invoke(this);
        
        // Stop the character if enabled
        if (stopOnFinish)
        {
            if (useDeceleration && characterMover != null)
            {
                characterMover.StartDeceleration(decelerationDuration);
            }
            else
            {
                StopCharacter();
            }
        }
    }
    
    private void StopCharacter()
    {
        if (characterMover != null)
        {
            // Store the current state
            wasMoving = characterMover.enabled;
            
            // Stop the character
            characterMover.enabled = false;
            
            Debug.Log($"{gameObject.name} has stopped at the finish line.");
            
        }
        else
        {
            Debug.LogWarning($"CharacterMover not found on {gameObject.name}. Cannot stop character.");
        }
    }
    

    
    // Public methods for external control
    public void SetFinishTag(string newTag)
    {
        finishTag = newTag;
    }
    
    public void ResetFinishState()
    {
        hasFinished = false;
        
        // Stop any ongoing deceleration and reset speed
        if (characterMover != null)
        {
            characterMover.StopDeceleration();
            characterMover.ResetSpeed();
            
            // Restore character movement if it was previously moving
            if (wasMoving)
            {
                characterMover.enabled = true;
            }
        }
    }
    
    public void ForceFinish()
    {
        if (!hasFinished)
        {
            OnFinishReached();
        }
    }
    
    public void StopCharacterManually()
    {
        StopCharacter();
    }
    
    public void ResumeCharacter()
    {
        if (characterMover != null)
        {
            characterMover.enabled = true;
            wasMoving = true;
        }
    }
    
    // Method to check if character is currently moving
    public bool IsCharacterMoving()
    {
        return characterMover != null && characterMover.enabled;
    }
    
    // Method to get finish status
    public bool GetFinishStatus()
    {
        return hasFinished;
    }
    
    // Deceleration control methods
    public void SetDecelerationDuration(float duration)
    {
        decelerationDuration = Mathf.Max(0.1f, duration);
    }
    
    public void EnableDeceleration(bool enable)
    {
        useDeceleration = enable;
    }
    
    public float GetCurrentSpeed()
    {
        if (characterMover != null)
        {
            return characterMover.GetMoveSpeed();
        }
        return 0f;
    }
    
    public bool IsDecelerating()
    {
        return characterMover != null && characterMover.IsDecelerating;
    }
}
