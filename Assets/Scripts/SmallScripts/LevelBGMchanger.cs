using System.Collections;
using UnityEngine;

public class LevelBGMchanger : MonoBehaviour
{
    private bool hasPlayed = false;


    void OnTriggerStay2D(Collider2D other) {
        if (!hasPlayed && other.gameObject.tag == "Player") {
            string sceneName = gameObject.scene.name;
            hasPlayed = true;

            switch (sceneName) {
                case "Level1":
                    AudioManager.PlayMusic(SoundID.LevelTheme);
                    if (Camera.main.backgroundColor != GameManager.instance.level1Color)
                        StartCoroutine(FadeToColor(GameManager.instance.level1Color));
                    break;
                case "Level2":
                    AudioManager.PlayMusic(SoundID.Level2Theme);
                    if (Camera.main.backgroundColor != GameManager.instance.level2Color)
                        StartCoroutine(FadeToColor(GameManager.instance.level2Color));
                    break;
                case "Level3":
                    AudioManager.PlayMusic(SoundID.Level3Theme);
                    if (Camera.main.backgroundColor != GameManager.instance.level3Color)
                        StartCoroutine(FadeToColor(GameManager.instance.level3Color));
                    break;
                case "Rest":
                    AudioManager.PlayMusic(SoundID.RestTheme);
                    break;
            }
        }

        // not happy but setting spawn here for consistency
        if (other.gameObject.tag == "Player") {
            string sceneName = gameObject.scene.name;
            if (sceneName == "Rest") return;
            GameManager.instance.currentSpawnPoint = GameObject.Find("SpawnPoint").transform;
        }
    }

    IEnumerator FadeToColor(Color color, float fadeTime = 1f) {
        float t = 0f;
        Color startColor = Camera.main.backgroundColor;
        while (t < fadeTime) {
            t += Time.deltaTime;
            Camera.main.backgroundColor = Color.Lerp(startColor, color, t / fadeTime);
            yield return null;
        }
        Camera.main.backgroundColor = color;
    }

    public void Reset()
    {
        hasPlayed = false;
    } 
}
