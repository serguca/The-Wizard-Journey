using System.Collections.Generic;
using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    [Header("Inventory Settings")]
    [SerializeField] private int inventorySize = 20;

    private List<InventorySlot> inventorySlots;
    public static Inventory Instance { get; private set; }

    // Eventos para UI
    public static event Action<Item, int> OnItemAdded;
    public static event Action<Item, int> OnItemRemoved;
    public static event Action OnInventoryChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeInventory();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeInventory()
    {
        inventorySlots = new List<InventorySlot>();
        for (int i = 0; i < inventorySize; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }

    public bool AddItem(Item item, int quantity = 1)
    {
        if (item == null) return false;

        // Buscar slot existente si es stackable
        if (item.isStackable)
        {
            foreach (var slot in inventorySlots)
            {
                if (slot.item == item && slot.quantity < item.maxStackSize)
                {
                    int spaceLeft = item.maxStackSize - slot.quantity;
                    int amountToAdd = Mathf.Min(quantity, spaceLeft);
                    slot.quantity += amountToAdd;
                    quantity -= amountToAdd;

                    OnItemAdded?.Invoke(item, amountToAdd);
                    OnInventoryChanged?.Invoke();

                    if (quantity <= 0) return true;
                }
            }
        }

        // Buscar slot vacío
        while (quantity > 0)
        {
            InventorySlot emptySlot = GetEmptySlot();
            if (emptySlot == null) return false; // Inventario lleno

            int amountToAdd = item.isStackable ? Mathf.Min(quantity, item.maxStackSize) : 1;
            emptySlot.item = item;
            emptySlot.quantity = amountToAdd;
            quantity -= amountToAdd;

            OnItemAdded?.Invoke(item, amountToAdd);
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool HasItem(Item item, int quantity = 1)
    {
        if (item == null) return false;

        int totalQuantity = 0;
        foreach (var slot in inventorySlots)
        {
            if (slot.item == item)
            {
                totalQuantity += slot.quantity;
                if (totalQuantity >= quantity) return true;
            }
        }
        return false;
    }

    public bool RemoveItem(Item item, int quantity = 1)
    {
        if (!HasItem(item, quantity)) return false;

        int remainingToRemove = quantity;
        for (int i = inventorySlots.Count - 1; i >= 0; i--)
        {
            var slot = inventorySlots[i];
            if (slot.item == item)
            {
                if (slot.quantity <= remainingToRemove)
                {
                    remainingToRemove -= slot.quantity;
                    OnItemRemoved?.Invoke(item, slot.quantity);
                    slot.Clear();
                }
                else
                {
                    slot.quantity -= remainingToRemove;
                    OnItemRemoved?.Invoke(item, remainingToRemove);
                    remainingToRemove = 0;
                }

                if (remainingToRemove <= 0) break;
            }
        }

        OnInventoryChanged?.Invoke();
        return true;
    }

    public int GetItemCount(Item item)
    {
        int totalQuantity = 0;
        foreach (var slot in inventorySlots)
        {
            if (slot.item == item)
            {
                totalQuantity += slot.quantity;
            }
        }
        return totalQuantity;
    }

    private InventorySlot GetEmptySlot()
    {
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty) return slot;
        }
        return null;
    }

    public List<InventorySlot> GetInventorySlots()
    {
        return new List<InventorySlot>(inventorySlots);
    }

    public bool IsFull()
    {
        return GetEmptySlot() == null;
    }
}

[Serializable]
public class InventorySlot
{
    public Item item;
    public int quantity;

    public bool IsEmpty => item == null || quantity <= 0;

    public void Clear()
    {
        item = null;
        quantity = 0;
    }
}