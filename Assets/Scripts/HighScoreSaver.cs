using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class HighScoreSaver : MonoBehaviour
{
    public float highScore = 0;
    public float currentScore = 0f;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        TextMeshProUGUI highScoreText = GameObject.FindWithTag("HighScore").GetComponent<TextMeshProUGUI>();
        highScoreText.SetText(GetHighScore().ToString());
    }

    public void SetHighScore()
    {
        PlayerStats pStats = FindAnyObjectByType<PlayerStats>();
        if (pStats.score > highScore)
        {
            PlayerPrefs.SetFloat("HighScore", pStats.score);
        }
    }
    
    public float GetHighScore()
    {
        return PlayerPrefs.GetFloat("HighScore");
    }
    
    
}
