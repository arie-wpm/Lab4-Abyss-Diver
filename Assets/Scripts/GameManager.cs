using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        GameStateManager.Instance.OnStateChange += HandleOnStateChange;
    }
    void OnEnable()
    {
        GameStateManager.Instance.OnStateChange += HandleOnStateChange;
    }

    void OnDisable()
    {
        GameStateManager.Instance.OnStateChange -= HandleOnStateChange;
    }
    
    void HandleOnStateChange(GameState state)
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
}
