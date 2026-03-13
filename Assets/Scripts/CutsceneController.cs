using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public bool loopCutscene = true;

    [SerializeField]
    private float skipToOnStart;
    private PlayableDirector director;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        director = GetComponent<PlayableDirector>();
    }

    public void CheckLoop()
    {
        if (loopCutscene)
        {
            director.time = 0;
        }
    }

    public void ContinueGame()
    {
        loopCutscene = false;
        director.time = skipToOnStart;
    }

    public void SwitchtoMainScene()
    {
        SceneManager.LoadScene("Main");
    }
}
