using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject PauseScreen { get; private set; }
    public GameObject GameOverScreen { get; private set; }

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

    public void SetGameOverScreen(GameObject gameOverScreen)
    {
        GameOverScreen = gameOverScreen;
    }
}