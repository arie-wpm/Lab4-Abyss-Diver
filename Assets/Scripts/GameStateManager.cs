using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public enum GameState
    {
        Null,
        StartMenu,
        Play,
        Pause,
        Fail,
        Win
    }

    public static GameStateManager Instance;
    public GameState CurrentGameState { get; private set; }
    public event Action<GameState> OnStateChange;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        GameStateManager.Instance.OnStateChange += HandleOnOnStateChange;
    }
    
    void HandleOnOnStateChange(GameState state)
    {
        switch (state)
        {
            case GameState.StartMenu:
                break;
            case GameState.Play:
                break;
            case GameState.Pause:
                break;
            case GameState.Fail:
                break;
            case GameState.Win:
                break;
        }
    }

    public void SetGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        CurrentGameState = newState;
        OnStateChange?.Invoke(newState);
    }
}
