using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;

    private Dictionary<string, string> items = new Dictionary<string, string>();
    private Dictionary<string, GameObject> itemPrefabs = new Dictionary<string, GameObject>();

    public delegate void InventoryChanged();
    public event InventoryChanged OnInventoryChanged;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddItem(string itemID, string displayName, GameObject prefab = null)
    {
        if (!items.ContainsKey(itemID))
        {
            items[itemID] = displayName;
            if (prefab != null)
                itemPrefabs[itemID] = prefab;
            OnInventoryChanged?.Invoke();
        }
    }

    public void RemoveItem(string itemID)
    {
        if (items.ContainsKey(itemID))
        {
            items.Remove(itemID);
            itemPrefabs.Remove(itemID);
            OnInventoryChanged?.Invoke();
        }
    }

    public bool HasItem(string itemID) => items.ContainsKey(itemID);

    public Dictionary<string, string> GetAllItems() => items;

    public GameObject GetPrefab(string itemID)
    {
        itemPrefabs.TryGetValue(itemID, out GameObject prefab);
        return prefab;
    }
}