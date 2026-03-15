using TMPro;
using UnityEngine;

public class MenuScore : MonoBehaviour
{
    private float highscore;
    void Start()
    {
        highscore = PlayerPrefs.GetFloat("HighScore", 0);
        var textBox = gameObject.GetComponent<TMP_Text>();
        textBox.text = "HighScore:\n" + highscore.ToString("00000");

    }
}
