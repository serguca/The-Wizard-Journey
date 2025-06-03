using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [Header("Door Settings")]
    [SerializeField] private float rotationAngle = 90f;
    [SerializeField] private float rotationSpeed = 2f;
    [SerializeField] private bool canBeOpened = true;
    [SerializeField] private bool canBeClosed = false;
    [SerializeField] private float detectionRadius = 3f; // Radio de detección

    [Header("Key Requirement")]
    [SerializeField] private bool requiresKey = false;
    [SerializeField] private Item requiredKey; // La llave específica que necesita
    [SerializeField] private bool consumeKeyOnUse = true; // Si la llave se consume al usar

    private bool isOpen = false;
    private bool isRotating = false;
    private Vector3 closedRotation;
    private Vector3 openRotation;
    private Transform player;
    private float sqrDetectionRadius; // Para evitar sqrt
    
    private bool HasRequiredKey()
    {
        return requiredKey != null && Inventory.Instance.HasItem(requiredKey, 1);
    }

    void Start()
    {
        enabled = canBeOpened;
        player = GameObject.FindGameObjectWithTag("Player").transform;

        sqrDetectionRadius = detectionRadius * detectionRadius;

        closedRotation = transform.eulerAngles;
        // Rotación cuand esté abierta
        openRotation = closedRotation + new Vector3(0, rotationAngle, 0);
    }

    void Update()
    {
        if (IsPlayerNearby() && Input.GetKeyDown(KeyCode.E) && !isRotating)
        {
            ToggleDoor();
        }
    }

    private bool IsPlayerNearby()
    {
        if (player == null) return false;

        // Usar sqrMagnitude para evitar sqrt (más eficiente)
        Vector3 direction = player.position - transform.position;
        return direction.sqrMagnitude <= sqrDetectionRadius;
    }

    private void ToggleDoor()
    {
        if (!canBeClosed && isOpen) return;
        if (requiresKey && !isOpen)
        {
            if (!HasRequiredKey()) return;
            if (consumeKeyOnUse)
                Inventory.Instance.RemoveItem(requiredKey, 1);
        }

        isOpen = !isOpen;
        StartCoroutine(RotateDoor());
    }

    private IEnumerator RotateDoor()
    {
        isRotating = true;

        Vector3 startRotation = transform.eulerAngles;
        Vector3 targetRotation = isOpen ? openRotation : closedRotation;

        float elapsedTime = 0f;
        float duration = 1f / rotationSpeed;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / duration;

            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, progress);

            yield return null;
        }

        // Asegurar que llegue exactamente a la rotación final
        transform.eulerAngles = targetRotation;
        isRotating = false;
        // if(!canBeClosed) enabled = false; // Desactivar el script si no se puede cerrar
    }
}