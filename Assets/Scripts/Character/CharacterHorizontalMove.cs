using UnityEngine;

public class CharacterHorizontalMove : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;
    
    [Header("References")]
    [SerializeField] private InputManager inputManager;
    
    private CharacterMover _characterMover;

    private void Awake()
    {
        if (inputManager == null)
        {
            inputManager = FindObjectOfType<InputManager>();
        }
        
        _characterMover = GetComponent<CharacterMover>();
    }

    private void Update()
    {
        if (_characterMover != null && !_characterMover.enabled) return;
        
        if (inputManager != null)
        {
            MoveHorizontally(inputManager.HorizontalDelta);
        }
    }

    private void MoveHorizontally(float delta)
    {
        if (Mathf.Approximately(delta, 0f)) return;

        Vector3 newPosition = transform.position;
        newPosition.x += delta * moveSpeed * Time.deltaTime;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        transform.position = newPosition;
    }

    public void SetHorizontalLimits(float min, float max)
    {
        minX = min;
        maxX = max;
    }
}


