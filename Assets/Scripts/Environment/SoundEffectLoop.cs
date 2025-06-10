using UnityEngine;

// Creado originalmente para el sonido de la lava y las cuchillas
public class SoundEffectLoop : MonoBehaviour
{
    [SerializeField] private AudioClip sound;
    [SerializeField] private float maxDistance = 20f;
    private Transform player;
    private AudioSource currentLoopedSound;
    private bool isPlaying = false;

    private void Start()
    {
        // Buscar el player al inicio
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null) Debug.LogWarning("SoundEffectLoop: Player not found.");
    }
    
    private void Update()
    {
        if (player != null)
        {
            float distance = Vector3.Distance(transform.position, player.position);
            
            // Si el jugador está cerca y el sonido no está reproduciéndose, activarlo
            if (distance <= maxDistance && !isPlaying)
            {
                PlayLoopedSound();
            }
            // Si el jugador está lejos y el sonido está reproduciéndose, desactivarlo
            else if (distance > maxDistance && isPlaying)
            {
                StopLoopedSound();
            }
        }
    }
    
    public void PlayLoopedSound()
    {
        if (!isPlaying && SoundManager.Instance != null)
        {
            currentLoopedSound = SoundManager.Instance.PlayLoopedSound(sound, transform.position);
            isPlaying = true;
        }
    }
    
    public void StopLoopedSound()
    {
        if (isPlaying && SoundManager.Instance != null)
        {
            SoundManager.Instance.StopLoopedSound(currentLoopedSound);
            currentLoopedSound = null;
            isPlaying = false;
        }
    }
    
    private void OnDestroy()
    {
        StopLoopedSound();
    }
}