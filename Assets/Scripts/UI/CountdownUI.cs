using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private Animation countdownAnimation;
    
    [Header("Animation Settings")]
    [SerializeField] private AnimationClip countdownAnimationClip;
    
    [Header("Text Settings")]
    [SerializeField] private string countdownFormat = "{0}";
    [SerializeField] private string goText = "GO!";
    [SerializeField] private float goTextDisplayTime = 1f;
    
    [Header("Game Manager Reference")]
    [SerializeField] private GameManager gameManager;
    
    private Coroutine goTextCoroutine;
    
    private void Awake()
    {
        // Auto-find GameManager if not assigned
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        
        if (countdownText == null)
        {
            countdownText = GetComponentInChildren<TextMeshProUGUI>();
        }
        
        if (countdownAnimation == null)
        {
            countdownAnimation = GetComponent<Animation>();
        }
        
        if (gameManager != null)
        {
            gameManager.OnCountdownTick.AddListener(OnCountdownTick);
            gameManager.OnGameStart.AddListener(OnGameStart);
        }
        
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnCountdownTick.RemoveListener(OnCountdownTick);
            gameManager.OnGameStart.RemoveListener(OnGameStart);
        }
    }


    public void OnCountdownTick(int countdownValue)
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = string.Format(countdownFormat, countdownValue);
            PlayCountdownAnimation();
        }
    }
    
    public void OnGameStart()
    {
        ShowGoText();
    }
    
    private void ShowGoText()
    {
        if (countdownText != null)
        {
            countdownText.text = goText;
            PlayCountdownAnimation();
            
            // Hide the text after a delay
            if (goTextCoroutine != null)
            {
                StopCoroutine(goTextCoroutine);
            }
            goTextCoroutine = StartCoroutine(HideCountdownTextAfterDelay(goTextDisplayTime));
        }
    }
    
    private void PlayCountdownAnimation()
    {
        if (countdownAnimation != null && countdownAnimationClip != null)
        {
            // Play the assigned animation clip
            countdownAnimation.Play(countdownAnimationClip.name);
        }
        else if (countdownAnimation == null)
        {
            Debug.LogWarning("No Animation component found on CountdownUI GameObject.");
        }
        else if (countdownAnimationClip == null)
        {
            Debug.LogWarning("No AnimationClip assigned to CountdownUI. Please assign an animation clip in the inspector.");
        }
    }
    
    private IEnumerator HideCountdownTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
    }
    
    // Public methods for manual control
    public void ShowCountdown(int value)
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(true);
            countdownText.text = string.Format(countdownFormat, value);
            PlayCountdownAnimation();
        }
    }
    
    public void ShowGo()
    {
        ShowGoText();
    }
    
    public void HideCountdown()
    {
        if (countdownText != null)
        {
            countdownText.gameObject.SetActive(false);
        }
        
        if (goTextCoroutine != null)
        {
            StopCoroutine(goTextCoroutine);
            goTextCoroutine = null;
        }
    }
    
    // Editor helper method
    [ContextMenu("Test Countdown Animation")]
    private void TestCountdownAnimation()
    {
        if (countdownAnimation != null && countdownAnimationClip != null)
        {
            countdownAnimation.Play(countdownAnimationClip.name);
        }
        else
        {
            Debug.LogWarning("No Animation component or AnimationClip assigned for testing.");
        }
    }
}
