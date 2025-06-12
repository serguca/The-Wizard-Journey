using System;
using UnityEngine;
using UnityEngine.Animations;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private string interactionPrompt = "Press E to interact";
    [SerializeField] private AudioClip doorOpenSound; // Sonido al abrir la puerta
    [SerializeField] private LookAtConstraint lookAtConstraint;
    private Transform player;
    private float sqrDetectionRadius;
    private Boolean hasInteracted = false;
    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sqrDetectionRadius = detectionRadius * detectionRadius;

        SetupLookAtConstraint();
    }

    private void SetupLookAtConstraint()
    {
        if (lookAtConstraint != null && player != null)
        {
            // Crear una nueva fuente de constraint
            ConstraintSource constraintSource = new ConstraintSource
            {
                sourceTransform = player,
                weight = 1f
            };
            
            // Añadir la fuente al constraint
            lookAtConstraint.AddSource(constraintSource);
            
            // Configurar el constraint
            lookAtConstraint.constraintActive = true;
            
            Debug.Log($"Look At Constraint configurado para seguir al player en {gameObject.name}");
        }
        else if (lookAtConstraint == null)
        {
            Debug.LogWarning($"Look At Constraint no asignado en {gameObject.name}");
        }
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