// PlayerInventory.cs
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory Instance;
    private HashSet<string> keys = new HashSet<string>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddKey(string keyID)
    {
        keys.Add(keyID);
        Debug.Log($"Picked up key: {keyID}");
    }

    public bool HasKey(string keyID) => keys.Contains(keyID);
}