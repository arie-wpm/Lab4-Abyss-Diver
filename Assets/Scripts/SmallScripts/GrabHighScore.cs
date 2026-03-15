using System;
using TMPro;
using UnityEngine;

public class GrabHighScore : MonoBehaviour
{
    private TextMeshProUGUI highScoreText;
    private void Awake()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        HighScoreSaver highScoreSaver = FindAnyObjectByType<HighScoreSaver>();
        highScoreSaver.SetHighScore();
        highScoreText.text = "High Score: " + highScoreSaver.GetHighScore().ToString();
    }
}
