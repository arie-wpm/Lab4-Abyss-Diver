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
        GameStateManager.Instance.SetGameState(GameState.Play);
    }

    private void ResetLevelBtnPress()
    {
        //reset level
        GameManager.instance.RestartCurrentLevel();
    }

    private void ReturnToTitleBtnPress()
    {
        GameManager.instance.RestartToTitle();
    }
}
