using UnityEngine;

public class PickableItem : Interactable
{
    [Header("Detection Settings")]
    
    [Header("Item Data")]
    [SerializeField] private Item keyItem; // ← AQUÍ SE ASIGNA EN EL INSPECTOR
    private bool isPickedUp = false;

    override protected void Start()
    {
        base.Start();
        // Validar que el keyItem esté asignado
        if (keyItem == null)
        {
            Debug.LogError($"Key {gameObject.name} doesn't have an Item assigned!");
        }
    }

    private void Update()
    {
        if (IsPlayerNearby() && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    
    override protected void Interact()
    {
        base.Interact();
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


}