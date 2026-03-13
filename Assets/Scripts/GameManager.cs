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
}
