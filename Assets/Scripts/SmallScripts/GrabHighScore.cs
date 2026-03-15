using System;
using TMPro;
using UnityEngine;

public class GrabHighScore : MonoBehaviour
{
    private TextMeshProUGUI highScoreText;
    private void OnEnable()
    {
        highScoreText = GetComponent<TextMeshProUGUI>();
        HighScoreSaver highScoreSaver = FindAnyObjectByType<HighScoreSaver>();
        highScoreText.text = "High Score: " + highScoreSaver.GetHighScore().ToString();
    }
}
