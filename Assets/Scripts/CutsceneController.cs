using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class CutsceneController : MonoBehaviour
{
    public bool loopCutscene = true;

    [SerializeField]
    private float skipToOnStart;

    [SerializeField]
    private float resetPoint = 0;
    private PlayableDirector director;

    [SerializeField]
    private TMP_Text scoreUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        scoreUI.text = "Final Score: " + PlayerStats.GlobalPlayerStats.score.ToString("00000");
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
        AudioManager.Play(SoundID.TitleSelect);
        loopCutscene = false;
        director.time = skipToOnStart;
    }

    public void SwitchtoMainScene()
    {
        SceneManager.LoadScene("Main");
    }

    public void SwitchtoOpeningScene()
    {
        SceneManager.LoadScene("Opening");
    }
}
