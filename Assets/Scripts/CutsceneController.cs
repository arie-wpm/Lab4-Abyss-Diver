using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public bool loopCutscene = true;

    [SerializeField]
    private float skipToOnStart;

    [SerializeField]
    private float resetPoint = 0;
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
            director.time = resetPoint;
        }
    }

    public void ContinueGame()
    {
        loopCutscene = false;
        director.time = skipToOnStart;
    }
}
