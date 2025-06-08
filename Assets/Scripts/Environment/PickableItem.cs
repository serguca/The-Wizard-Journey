using UnityEngine;

public class PickableItem : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 3f;
    
    [Header("Item Data")]
    [SerializeField] private Item keyItem; // ← AQUÍ SE ASIGNA EN EL INSPECTOR
    
    private bool isPickedUp = false;
    private Transform player;
    private float sqrDetectionRadius;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        sqrDetectionRadius = detectionRadius * detectionRadius;
        
        // Validar que el keyItem esté asignado
        if (keyItem == null)
        {
            Debug.LogError($"Key {gameObject.name} doesn't have an Item assigned!");
        }
    }

    void Update()
    {
        if (IsPlayerNearby() && Input.GetKeyDown(KeyCode.E))
        {
            PickUp();
        }
    }
    
    private void PickUp()
    {
        if (isPickedUp || keyItem == null) return;

        // Usar el sistema de inventario
        if (Inventory.Instance.AddItem(keyItem, 1))
        {
            isPickedUp = true;
            gameObject.SetActive(false);
            Debug.Log($"{keyItem.itemName} picked up!");
        }
        else
        {
            Debug.Log("Inventory is full!");
        }
    }

    private bool IsPlayerNearby()
    {
        if (player == null) return false;
        Vector3 direction = player.position - transform.position;
        return direction.sqrMagnitude <= sqrDetectionRadius;
    }
}