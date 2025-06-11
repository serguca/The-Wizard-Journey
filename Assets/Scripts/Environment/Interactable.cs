using System;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 3f;
    [SerializeField] private string interactionPrompt = "Press E to interact";
    [SerializeField] private AudioClip doorOpenSound;
    private Player playerScript;
    private float sqrDetectionRadius;
    private Boolean hasInteracted = false;
    private bool playerWasNearby = false; // Nuevo: para detectar cambios
    
    protected virtual void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            playerScript = playerObject.GetComponent<Player>();
        }
        
        sqrDetectionRadius = detectionRadius * detectionRadius;
    }

    private void Update()
    {
        bool playerIsNearby = IsPlayerNearby();
        
        // Solo ejecutar callbacks cuando cambia el estado
        if (playerIsNearby && !playerWasNearby)
        {
            OnPlayerEnter();
        }
        else if (!playerIsNearby && playerWasNearby)
        {
            OnPlayerExit();
        }
        
        playerWasNearby = playerIsNearby;
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
        Debug.Log($"Player entered range of: {gameObject.name}");
        if (playerScript != null)
        {
            playerScript.SetPickupText(true);
        }
    }

    public virtual void OnPlayerExit()
    {
        Debug.Log($"Player exited range of: {gameObject.name}");
        if (playerScript != null)
        {
            playerScript.SetPickupText(false);
        }
    }

    protected bool IsPlayerNearby()
    {
        if (playerScript == null) return false;
        Vector3 direction = playerScript.transform.position - transform.position;
        return direction.sqrMagnitude <= sqrDetectionRadius;
    }
}