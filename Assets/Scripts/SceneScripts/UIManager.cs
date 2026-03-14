using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject PauseScreen;
    public GameOverMenuManager GameOverScreen;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}