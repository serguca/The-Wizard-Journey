using System.Collections;
using UnityEngine;

// Creado originalmente para el sonido de la lava y las cuchillas
public class PeriodicSoundEffect : MonoBehaviour
{
    [SerializeField] private float soundCooldown = 1f;
    [SerializeField] private AudioClip sound;
    [SerializeField] private float maxSoundDistance = 20f;
    private Transform player;
    private float maxSoundDistanceSqr; // Versión al cuadrado para comparar
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        maxSoundDistanceSqr = maxSoundDistance * maxSoundDistance; // Pre-calcular
        StartCoroutine(SoundCooldown());
    }

    private IEnumerator SoundCooldown()
    {
        while (true)
        {
            if (player != null)
            {
                Vector3 directionToPlayer = player.position - transform.position;
                float sqrDistance = directionToPlayer.sqrMagnitude;
                
                if (sqrDistance <= maxSoundDistanceSqr)
                {
                    SoundManager.Instance.PlaySound(sound, transform.position);
                }
            }
            yield return new WaitForSeconds(soundCooldown);
        }
    }
}