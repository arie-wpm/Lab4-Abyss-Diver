using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.StopMusic();
            HighScoreSaver highScoreSaver = FindAnyObjectByType<HighScoreSaver>();
            highScoreSaver.currentScore = FindAnyObjectByType<PlayerStats>().score;
            highScoreSaver.SetHighScore();
            SceneManager.LoadScene("Ending");
        }
    }
}
