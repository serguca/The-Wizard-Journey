using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private int poolSize = 20;
    [SerializeField] private AudioSource audioSourcePrefab;
    private Queue<AudioSource> audioPool = new Queue<AudioSource>();
    private List<AudioSource> activeAudioSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            AudioSource source = Instantiate(audioSourcePrefab, transform);
            source.gameObject.SetActive(false);
            audioPool.Enqueue(source);
        }
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 0.25f)
    {
        AudioSource source = GetPooledAudioSource();
        if (source != null)
        {
            source.transform.position = position;
            source.clip = clip;
            source.volume = volume;
            source.gameObject.SetActive(true);
            source.Play();
            Debug.Log($"Playing sound: {clip.name}");

            StartCoroutine(ReturnToPoolWhenFinished(source, clip.length));
        }
    }

    private AudioSource GetPooledAudioSource()
    {
        if (audioPool.Count > 0)
        {
            return audioPool.Dequeue();
        }

        // Si no hay disponibles, crea uno nuevo
        AudioSource newSource = Instantiate(audioSourcePrefab, transform);
        return newSource;
    }

    private IEnumerator ReturnToPoolWhenFinished(AudioSource source, float duration)
    {
        yield return new WaitForSeconds(duration);

        source.gameObject.SetActive(false);
        audioPool.Enqueue(source);
    }
    
    public AudioSource PlayLoopedSound(AudioClip clip, Vector3 position, float volume = 0.5f)
    {
        AudioSource source = GetPooledAudioSource();
        if (source != null)
        {
            source.transform.position = position;
            source.clip = clip;
            source.volume = volume;
            source.loop = true;
            source.gameObject.SetActive(true);
            source.Play();
            
            activeAudioSources.Add(source);
        }
        return source;
    }
    
    public void StopLoopedSound(AudioSource source)
    {
        if (source != null && activeAudioSources.Contains(source))
        {
            source.Stop();
            source.loop = false;
            source.gameObject.SetActive(false);
            activeAudioSources.Remove(source);
            audioPool.Enqueue(source);
        }
    }
}