using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Header("Input Settings")]
    [SerializeField] private float inputSensitivity = 1f;
    
    private Vector2 _lastInputPosition;
    private Vector2 _currentInputPosition;
    private bool _isInputActive;
    private float _horizontalDelta;

    public float HorizontalDelta => _horizontalDelta;
    public bool IsInputActive => _isInputActive;

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        _horizontalDelta = 0f;

#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseInput();
#elif UNITY_ANDROID || UNITY_IOS
        HandleTouchInput();
#else
        HandleTouchInput();
#endif
    }

    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _isInputActive = true;
            _lastInputPosition = Input.mousePosition;
            _currentInputPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            _currentInputPosition = Input.mousePosition;
            _horizontalDelta = (_currentInputPosition.x - _lastInputPosition.x) * inputSensitivity;
            _lastInputPosition = _currentInputPosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            _isInputActive = false;
            _horizontalDelta = 0f;
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _isInputActive = true;
                _lastInputPosition = touch.position;
                _currentInputPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                _currentInputPosition = touch.position;
                _horizontalDelta = (_currentInputPosition.x - _lastInputPosition.x) * inputSensitivity;
                _lastInputPosition = _currentInputPosition;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                _isInputActive = false;
                _horizontalDelta = 0f;
            }
        }
        else
        {
            _isInputActive = false;
            _horizontalDelta = 0f;
        }
    }
}










