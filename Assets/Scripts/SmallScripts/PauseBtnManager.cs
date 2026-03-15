using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseBtnManager : MonoBehaviour
{
    public static event Action<GameObject> OnPauseScreenReady;
    [SerializeField] private Button continueBtn;
    [SerializeField] private Button resetLvlBtn;
    [SerializeField] private Button returnToTitleBtn;

    void Awake()
    {
        UIManager.instance.PauseScreen = this.gameObject;
        OnPauseScreenReady?.Invoke(this.gameObject);
    }
    private void OnEnable()
    {
        continueBtn.onClick.AddListener(ContinueBtnPress);
        resetLvlBtn.onClick.AddListener(ResetLevelBtnPress);
        returnToTitleBtn.onClick.AddListener(ReturnToTitleBtnPress);
    }

    private void ContinueBtnPress()
    {
        AudioManager.Play(SoundID.PauseButtonClick);
        GameStateManager.Instance.SetGameState(GameState.Play);
    }

    private void ResetLevelBtnPress()
    {
        AudioManager.Play(SoundID.PauseButtonClick);
        GameManager.instance.RestartCurrentLevel();
    }

    private void ReturnToTitleBtnPress()
    {
        AudioManager.Play(SoundID.PauseButtonClick);
        GameManager.instance.RestartToTitle();
    }
}
