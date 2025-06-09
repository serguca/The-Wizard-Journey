using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private AudioClip musicClip;
    void Start()
    {
        MusicManager.Instance.PlayMusic(musicClip);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
