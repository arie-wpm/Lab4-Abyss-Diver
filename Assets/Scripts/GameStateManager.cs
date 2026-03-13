using System;
using UnityEngine;

public enum GameState
{
    Null,
    StartMenu,
    Play,
    Pause,
    Fail,
    Win
}

public class GameStateManager : MonoBehaviour
{
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

    public void SetGameState(GameState newState)
    {
        if (CurrentGameState == newState) return;

        CurrentGameState = newState;
        OnStateChange?.Invoke(newState);
    }
}
