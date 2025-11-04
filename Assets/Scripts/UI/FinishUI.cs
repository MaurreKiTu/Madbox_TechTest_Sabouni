using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class FinishUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI finishText;
        [SerializeField] private Animation countdownAnimation;
        
        [Header("Animation Settings")]
        [SerializeField] private AnimationClip finishAnimationClip; 
        
        [Header("Game Manager Reference")]
        [SerializeField] private GameManager gameManager;

        private void Awake()
        {
            if (gameManager != null)
            {
                gameManager.OnPlayerFinish.AddListener(OnPlayerFinish);    
            }
            
        }

        private void OnDestroy()
        {
            if (gameManager != null)
            {
                gameManager.OnPlayerFinish.RemoveListener(OnPlayerFinish);    
            }
        }

        private void OnPlayerFinish(CharacterMover characterMover, int position)
        {
            if (!characterMover.IsPlayer)
            {
                return;
            }
            finishText.text = $"{characterMover.name} finished at {position}";
            countdownAnimation.Play(finishAnimationClip.name);
        }
    }
}