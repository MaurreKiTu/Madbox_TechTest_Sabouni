using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    [SerializeField] private float countdownDuration = 3f;
    [SerializeField] private bool startGameOnStart = true;
    
    [Header("Player Management")]
    [SerializeField] private List<CharacterMover> players = new List<CharacterMover>();
    
    [Header("Events")]
    [SerializeField] private UnityEvent onGameStart;
    [SerializeField] private UnityEvent<int> onCountdownTick;
    [SerializeField] private UnityEvent<CharacterMover, int> onPlayerFinish;
    [SerializeField] private UnityEvent onCountdownComplete;
    
    // Public events for external subscription
    public UnityEvent OnGameStart => onGameStart;
    public UnityEvent<int> OnCountdownTick => onCountdownTick;
    public UnityEvent OnCountdownComplete => onCountdownComplete;
    public UnityEvent<CharacterMover, int> OnPlayerFinish => onPlayerFinish;
    
    private bool _gameStarted = false;
    private bool _countdownActive = false;
    private Coroutine _countdownCoroutine;
    private List<CharacterMover> _finishedCharacters = new List<CharacterMover>();
    
    
    void Awake()
    {
        StopGame();

    }

    private void Start()
    {
        if (startGameOnStart)
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        if (_gameStarted || _countdownActive) return;
        
        _countdownActive = true;
        _countdownCoroutine = StartCoroutine(CountdownCoroutine());
    }
    
    public void StopGame()
    {
        _gameStarted = false;
        _countdownActive = false;
        
        if (_countdownCoroutine != null)
        {
            StopCoroutine(_countdownCoroutine);
            _countdownCoroutine = null;
        }
        
        foreach (var player in players)
        {
            if (player != null)
            {
                player.enabled = false;
            }
        }
    }
    
    public void RestartGame()
    {
        StopGame();
        StartGame();
    }
    
    private IEnumerator CountdownCoroutine()
    {
        Debug.Log("Game starting in 3 seconds...");
        
        // Countdown from 3 to 1
        for (int i = 3; i >= 1; i--)
        {
            onCountdownTick?.Invoke(i);
            Debug.Log($"Starting in {i}...");
            yield return new WaitForSeconds(1f);
        }
        
        // Start the game
        StartPlayers();
        _countdownActive = false;
        _gameStarted = true;
        
        onCountdownComplete?.Invoke();
        onGameStart?.Invoke();
        
        Debug.Log("Game started! Players are moving!");
    }
    
    private void StartPlayers()
    {
        foreach (var player in players)
        {
            if (player != null)
            {
                player.enabled = true;
                CharacterFinishDetector characterFinishDetector = player.GetComponent<CharacterFinishDetector>();
                characterFinishDetector.onFinishReached += OnFinishReached;
            }
        }
    }

    private void OnFinishReached(CharacterFinishDetector characterFinishDetector)
    {
        Debug.Log("On finish reached for player " + characterFinishDetector.name);
        characterFinishDetector.onFinishReached -= OnFinishReached;
        _finishedCharacters.Add(characterFinishDetector.CharacterMover);
        onPlayerFinish.Invoke(characterFinishDetector.CharacterMover, _finishedCharacters.Count);
    }

    // Player management methods
    public void AddPlayer(CharacterMover player)
    {
        if (player != null && !players.Contains(player))
        {
            players.Add(player);
            // Disable the player until game starts
            player.enabled = false;
        }
    }
    
    
    // Editor helper method to find players automatically
    [ContextMenu("Find All Players")]
    private void FindAllPlayers()
    {
        players.Clear();
        CharacterMover[] foundPlayers = FindObjectsOfType<CharacterMover>();
        players.AddRange(foundPlayers);
        
        // Disable all players initially
        foreach (var player in players)
        {
            if (player != null)
            {
                player.enabled = false;
            }
        }
        
        Debug.Log($"Found {players.Count} players");
    }
}
