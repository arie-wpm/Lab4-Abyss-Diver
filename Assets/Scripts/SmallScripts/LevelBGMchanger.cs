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
                        StartCoroutine(FadeToColor(GameManager.instance.level1Color, GameManager.instance.level1LightIntensity, GameManager.instance.level1BloomIntensity));
                    break;
                case "Level2":
                    AudioManager.PlayMusic(SoundID.Level2Theme);
                    if (Camera.main.backgroundColor != GameManager.instance.level2Color)
                        StartCoroutine(FadeToColor(GameManager.instance.level2Color, GameManager.instance.level2LightIntensity, GameManager.instance.level2BloomIntensity));
                    break;
                case "Level3":
                    AudioManager.PlayMusic(SoundID.Level3Theme);
                    if (Camera.main.backgroundColor != GameManager.instance.level3Color)
                        StartCoroutine(FadeToColor(GameManager.instance.level3Color, GameManager.instance.level3LightIntensity, GameManager.instance.level3BloomIntensity));
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

    IEnumerator FadeToColor(Color color, float lightIntensity, float bloomIntensity, float fadeTime = 1f) {
        float t = 0f;
        Color startColor = Camera.main.backgroundColor;
        float startLightIntensity = GameManager.instance.globalLight.intensity;
        float startBloomIntensity = GameManager.instance.globalBloom.intensity.value;
        
        while (t < fadeTime) {
            t += Time.deltaTime;
            Camera.main.backgroundColor = Color.Lerp(startColor, color, t / fadeTime);
            GameManager.instance.globalLight.intensity = Mathf.Lerp(startLightIntensity, lightIntensity, t);
            GameManager.instance.globalBloom.intensity.value = Mathf.Lerp(startBloomIntensity, bloomIntensity, t);
            yield return null;
        }
        Camera.main.backgroundColor = color;
        GameManager.instance.globalLight.intensity = lightIntensity;
        GameManager.instance.globalBloom.intensity.value = bloomIntensity;
    }

    public void Reset()
    {
        hasPlayed = false;
    } 
}
