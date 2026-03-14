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
                    break;
                case "Level2":
                    AudioManager.PlayMusic(SoundID.Level2Theme);
                    break;
                case "Level3":
                    AudioManager.PlayMusic(SoundID.Level3Theme);
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

    public void Reset()
    {
        hasPlayed = false;
    } 
}
