using UnityEngine;
using DG.Tweening;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    [Header("Game Manager Reference")]
    private GameManager gameManager;
    
    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private Ease animationEase = Ease.OutBounce;
    [SerializeField] private float offScreenYPosition = 1000f;
    
    private RectTransform _rectTransform;
    private Vector2 _targetPosition;
    private bool _isShowing = false;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        
        _targetPosition = _rectTransform.anchoredPosition;
        
        HideImmediate();
        
        if (gameManager != null)
        {
            gameManager.OnPlayerDefeated.AddListener(ShowGameOver);
        }
    }

    public void ShowGameOver()
    {
        if (_isShowing) return;
        
        _isShowing = true;
        
        gameObject.SetActive(true);
        
        _rectTransform.DOAnchorPosY(_targetPosition.y, animationDuration)
            .SetEase(animationEase)
            .SetUpdate(true);
    }

    public void HideGameOver()
    {
        _isShowing = false;
        
        _rectTransform.DOAnchorPosY(offScreenYPosition, animationDuration)
            .SetEase(Ease.InBack)
            .SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(false));
    }

    private void HideImmediate()
    {
        _rectTransform.anchoredPosition = new Vector2(_targetPosition.x, offScreenYPosition);
        gameObject.SetActive(false);
    }
    
    private void OnDestroy()
    {
        if (gameManager != null)
        {
            gameManager.OnPlayerDefeated.RemoveListener(ShowGameOver);
        }
        
        _rectTransform.DOKill();
    }
}

