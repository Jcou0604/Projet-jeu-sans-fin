using UnityEngine;

public class DropSystem : MonoBehaviour
{
    public static DropSystem Instance;
    public Transform playerTransform;
    public float dropDistance = 0.5f;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void DropItem(string itemID)
    {
        GameObject prefab = PlayerInventory.Instance.GetPrefab(itemID);

        if (prefab != null)
        {
            // Spawn it at the player's feet
            Vector3 dropPosition = playerTransform.position +
                                   Vector3.down * 0.5f;
            Instantiate(prefab, dropPosition, Quaternion.identity);
        }

        PlayerInventory.Instance.RemoveItem(itemID);
    }
}