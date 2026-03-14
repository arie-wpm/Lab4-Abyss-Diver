using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject PauseScreen { get; private set; }
    public GameOverMenuManager GameOverScreen { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPauseScreen(GameObject pauseScreen)
    {
        PauseScreen = pauseScreen;
    }

    public void SetGameOverScreen(GameOverMenuManager gameOverScreen)
    {
        GameOverScreen = gameOverScreen;
    }
}