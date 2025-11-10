using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CameraType
{
    Main,
    Race,
    Ending
}

public enum ShakeIntensity
{
    Light,
    Medium,
    Heavy
}

public class CameraManager : MonoBehaviour
{
    [Serializable]
    private class CameraEntry
    {
        public CameraType type;
        public CinemachineVirtualCamera camera;
    }
    
    [Header("Camera Setup")]
    [SerializeField] private List<CameraEntry> cameras = new List<CameraEntry>();
    [SerializeField] private CameraType defaultCamera = CameraType.Main;
    
    [Header("Game Manager Reference")]
    private GameManager _gameManager;
    
    [Header("Priority Settings")]
    private int _activeCameraPriority = 10;
    private int _inactiveCameraPriority = 0;

    [Header("Screen Shake Settings")]
    [SerializeField] private float lightShakeIntensity = 0.5f;
    [SerializeField] private float mediumShakeIntensity = 1f;
    [SerializeField] private float heavyShakeIntensity = 2f;

    private Dictionary<CameraType, CinemachineVirtualCamera> _cameraMap;
    private CameraType _currentCamera;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        if (_gameManager == null)
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        if (_gameManager != null)
        {
            _gameManager.OnCountdownComplete.AddListener(OnCountdownComplete);
        }
        
        InitializeShakeSystem();
        BuildCameraMap();
        SwitchCamera(defaultCamera);
    }

    private void OnDestroy()
    {
        if (_gameManager != null)
        {
            _gameManager.OnCountdownComplete.RemoveListener(OnCountdownComplete);
        }
    }

    private void OnCountdownComplete()
    {
        SwitchCamera(CameraType.Race);
    }

    private void BuildCameraMap()
    {
        _cameraMap = new Dictionary<CameraType, CinemachineVirtualCamera>();
        
        foreach (var entry in cameras)
        {
            if (entry.camera != null && !_cameraMap.ContainsKey(entry.type))
            {
                _cameraMap.Add(entry.type, entry.camera);
                entry.camera.Priority = _inactiveCameraPriority;
            }
        }
    }

    public void SwitchCamera(CameraType cameraType)
    {
        if (!_cameraMap.ContainsKey(cameraType))
        {
            Debug.LogWarning($"Camera type {cameraType} not found in CameraManager.");
            return;
        }

        foreach (var kvp in _cameraMap)
        {
            kvp.Value.Priority = _inactiveCameraPriority;
        }

        _cameraMap[cameraType].Priority = _activeCameraPriority;
        _currentCamera = cameraType;
    }

    public CameraType GetCurrentCamera()
    {
        return _currentCamera;
    }

    #region Screen Shake

    private void InitializeShakeSystem()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
        if (_impulseSource == null)
        {
            _impulseSource = gameObject.AddComponent<CinemachineImpulseSource>();
        }
    }

    public void Shake(ShakeIntensity intensity)
    {
        if (_impulseSource == null) return;
        
        float intensityValue = intensity switch
        {
            ShakeIntensity.Light => lightShakeIntensity,
            ShakeIntensity.Medium => mediumShakeIntensity,
            ShakeIntensity.Heavy => heavyShakeIntensity,
            _ => mediumShakeIntensity
        };
        
        _impulseSource.GenerateImpulse(intensityValue);
    }

    public void Shake(float customIntensity)
    {
        if (_impulseSource != null)
        {
            _impulseSource.GenerateImpulse(customIntensity);
        }
    }

    public static void TriggerShake(ShakeIntensity intensity)
    {
        CameraManager manager = FindObjectOfType<CameraManager>();
        if (manager != null)
        {
            manager.Shake(intensity);
        }
    }

    public static void TriggerShake(float customIntensity)
    {
        CameraManager manager = FindObjectOfType<CameraManager>();
        if (manager != null)
        {
            manager.Shake(customIntensity);
        }
    }

    #endregion
}





