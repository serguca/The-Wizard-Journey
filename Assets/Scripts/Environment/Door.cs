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

    private bool isOpen = false;
    private bool isRotating = false;
    private Vector3 closedRotation;
    private Vector3 openRotation;
    private Transform player;
    private float sqrDetectionRadius; // Para evitar sqrt

    void Start()
    {
        enabled = canBeOpened;
                // Encontrar al jugador
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
        // Pre-calcular el radio al cuadrado para evitar sqrt
        sqrDetectionRadius = detectionRadius * detectionRadius;

        // Guardar la rotación inicial (puerta cerrada)
        closedRotation = transform.eulerAngles;
        // Calcular la rotación cuando esté abierta
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
        // Si no se puede cerrar y ya está abierta, no hacer nada
        if (!canBeClosed && isOpen) return;

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

            // Interpolación suave
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, progress);

            yield return null;
        }

        // Asegurar que llegue exactamente a la rotación final
        transform.eulerAngles = targetRotation;
        isRotating = false;
        // if(!canBeClosed) enabled = false; // Desactivar el script si no se puede cerrar
    }
}