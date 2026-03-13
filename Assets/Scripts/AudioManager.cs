using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SoundID {
    PlayerMove,
    PlayerDash,
    Pickup,
    LevelTheme,
    Level2Theme,
    Level3Theme,
    RestTheme,
    TitleScreen,
    TitleSelect,
    EnemyMove,
    Death,
    GameOver,
    Hurt
}

[System.Serializable]
public class Sound {
    public SoundID id;
    public AudioClip[] clips;

    [Range(0f, 1f)] public float volume = 1f;
    public bool randomPitch;
    public float pitch = 1f;
}

public class AudioManager : MonoBehaviour {

    private static AudioManager instance;

    [Header("Debug")]
    [SerializeField] private bool enableDebug = false;    

    [Header("Volumes")]
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;
    [Range(0f, 1f)] public float uiVolume = 1f;

    [Header("Sound Library")]
    [SerializeField] private Sound[] sounds;

    private Dictionary<SoundID, Sound> soundMap;
    private List<AudioSource> sfxSources;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource uiSource;
    [SerializeField] int sfxPoolSize = 10;

    void Awake() {
        DebugTool.EnableLogging(nameof(AudioManager), enableDebug);
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        soundMap = new Dictionary<SoundID, Sound>();
        foreach (var s in sounds) {
            soundMap[s.id] = s;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        uiSource = gameObject.AddComponent<AudioSource>();

        CreateSFXPool();
    }

    void Start() {
        PlayMusic(SoundID.TitleScreen);
    }

    void CreateSFXPool() {
        sfxSources = new List<AudioSource>();
        for (int i = 0; i < sfxPoolSize; i++) {
            AudioSource src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;
            sfxSources.Add(src);
        }
    }

    AudioSource GetFreeSFXSource() {
        foreach (var src in sfxSources) {
            if (!src.isPlaying) return src;
        }
        return sfxSources[0];
    }

    AudioClip GetRandomClip(Sound sound) {
        return sound.clips[Random.Range(0, sound.clips.Length)];
    }

    public static void Play(SoundID id) {
        if (instance == null) return;

        if (!instance.soundMap.TryGetValue(id, out Sound sound)) {
            DebugTool.LogError($"Could not find sound with id: {id}.");
            return;
        }

        AudioSource src = instance.GetFreeSFXSource();
        float pitch = sound.pitch;
        if (sound.randomPitch) pitch = Random.Range(0.8f, 1.2f);
        src.pitch = pitch;
        src.volume = sound.volume * instance.sfxVolume;
        src.PlayOneShot(instance.GetRandomClip(sound));
    }

    public static void PlayMusic(SoundID id, float fadeTime = 0.5f) {
        if (instance == null) return;

        if (!instance.soundMap.TryGetValue(id, out Sound sound)) {
            DebugTool.LogError($"Could not find sound with id: {id}.");
            return;
        }
        instance.StartCoroutine(instance.CrossFadeMusic(instance.GetRandomClip(sound), fadeTime));
    }

    IEnumerator CrossFadeMusic(AudioClip newClip, float fadeTime) {
        float t = 0f;
        float startVolume = musicSource.volume;

        while (t < fadeTime) {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.clip = newClip;
        musicSource.Play();

        t = 0f;
        while (t < fadeTime) {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t / fadeTime);
            yield return null;
        }
        musicSource.volume = musicVolume;
    }

    public static void PlayUI(SoundID id) {
        if (instance == null) return;
        if (!instance.soundMap.TryGetValue(id, out Sound sound)) {
            DebugTool.LogError($"Could not find sound with id: {id}.");
            return;
        }
        instance.uiSource.pitch = sound.pitch;
        instance.uiSource.volume = sound.volume * instance.uiVolume;
        instance.uiSource.PlayOneShot(instance.GetRandomClip(sound));
    }

    public static void StopMusic() {
        if (instance == null) return;
        instance.musicSource.Stop();
    }

    public static void PauseMusic() {
        if (instance == null) return;
        instance.musicSource.Pause();
    }

    public static void ResumeMusic() {
        if (instance == null) return;
        instance.musicSource.UnPause();
    }
}
