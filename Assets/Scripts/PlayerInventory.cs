using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    // Stores item names with their display names
    private Dictionary<string, string> items = new Dictionary<string, string>();

    // Event so the UI updates automatically
    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(string itemID, string displayName)
    {
        if (!items.ContainsKey(itemID))
        {
            items[itemID] = displayName;
            Debug.Log($"Picked up: {displayName}");
            OnInventoryChanged?.Invoke();
        }
    }

    public void RemoveItem(string itemID)
    {
        if (items.ContainsKey(itemID))
        {
            items.Remove(itemID);
            OnInventoryChanged?.Invoke();
        }
    }

    public bool HasItem(string itemID) => items.ContainsKey(itemID);

    public Dictionary<string, string> GetAllItems() => items;
}