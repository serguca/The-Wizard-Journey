using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambientSource;

    private float originalMusicVolume = 0.5f;
    private bool isMusicMuted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(AudioClip clip, bool loop = true, float volume = 0.5f)
    {
        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayAmbient(AudioClip clip, bool loop = true)
    {
        ambientSource.clip = clip;
        ambientSource.loop = loop;
        ambientSource.Play();
    }

    public void ToggleMusicMute()
    {
        isMusicMuted = !isMusicMuted;
        musicSource.volume = isMusicMuted ? 0f : originalMusicVolume;
        
        Debug.Log($"Music {(isMusicMuted ? "muted" : "unmuted")}");
    }

    public void MuteMusic()
    {
        if (!isMusicMuted)
        {
            isMusicMuted = true;
            musicSource.volume = 0f;
            Debug.Log("Music muted");
        }
    }

    public void UnmuteMusic()
    {
        if (isMusicMuted)
        {
            isMusicMuted = false;
            musicSource.volume = originalMusicVolume;
            Debug.Log("Music unmuted");
        }
    }
    
        public void SetMusicVolume(float volume)
    {
        originalMusicVolume = volume;
        if (!isMusicMuted)
        {
            musicSource.volume = volume;
        }
    }
}