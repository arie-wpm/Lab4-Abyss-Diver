using TMPro;
using UnityEngine;

public class MenuScore : MonoBehaviour
{
    private int highscore;
    void Start()
    {
        if (PlayerPrefs.HasKey("HighScore"))
        {
            highscore = PlayerPrefs.GetInt("HighScore", 0);
        }
        
        var textBox = gameObject.GetComponent<TMP_Text>();
        textBox.text = "HighScore:\n" + highscore.ToString("00000");

    }
}
