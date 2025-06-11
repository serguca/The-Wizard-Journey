using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private string interactionPrompt = "Press E to interact";
    [SerializeField] private AudioClip doorOpenSound; // Sonido al abrir la puerta
    private Transform player;
    private float sqrDetectionRadius;
    private Boolean hasInteracted = false;
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sqrDetectionRadius = detectionRadius * detectionRadius;
    }


    protected virtual void Interact()
    {
        Debug.Log("Interacted with: " + gameObject.name);
        if (!hasInteracted && doorOpenSound != null)
        {
            SoundManager.Instance.PlaySound(doorOpenSound, transform.position);
            hasInteracted = true;
        }
    }

    public virtual void OnPlayerEnter()
    {
        // Mostrar prompt de interacción
    }

    public virtual void OnPlayerExit()
    {
        // Ocultar prompt de interacción
    }

    protected bool IsPlayerNearby()
    {
        if (player == null) return false;
        Vector3 direction = player.position - transform.position;
        return direction.sqrMagnitude <= sqrDetectionRadius;
    }
}