using System;
using TMPro;
using UnityEngine;

public class GameOverMenuManager : MonoBehaviour
{
    public static event Action<GameOverMenuManager> OnGameOverMenuReady;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text deathText;

    void Awake()
    {
        UIManager.instance.GameOverScreen = this;
        OnGameOverMenuReady?.Invoke(this);
    }

    public void SetScore(string score)
    {
        scoreText.text = "Score: " + score;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
