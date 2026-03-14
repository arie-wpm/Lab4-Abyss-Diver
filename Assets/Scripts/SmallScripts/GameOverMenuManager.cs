using System;
using TMPro;
using UnityEngine;

public class GameOverMenuManager : MonoBehaviour
{

    [SerializeField] private TMP_Text scoreText;
    void Awake()
    {
        UIManager.instance.SetGameOverScreen(this);
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
