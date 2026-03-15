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

    void Start()
    {
        director = GetComponent<PlayableDirector>();

        if (scoreUI != null && PlayerStats.GlobalPlayerStats != null)
        {
            scoreUI.SetText("Final Score: " + PlayerStats.GlobalPlayerStats.score.ToString());
        }
        else
        {
            if (scoreUI == null)
                Debug.LogWarning("CutsceneController: scoreUI is not assigned!");
            if (PlayerStats.GlobalPlayerStats == null)
                Debug.LogWarning("CutsceneController: GlobalPlayerStats is null!");
        }
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
