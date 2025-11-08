using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public enum CameraType
{
    Main,
    Race
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

    private Dictionary<CameraType, CinemachineVirtualCamera> _cameraMap;
    private CameraType _currentCamera;

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
}





