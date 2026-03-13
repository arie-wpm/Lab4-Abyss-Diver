using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // Scene check
    public static string currentScene;
    public static string previousScene;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
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
