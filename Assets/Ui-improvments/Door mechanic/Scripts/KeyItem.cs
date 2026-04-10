using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string keyID = "LockboxKey";
    public string displayName = "Lockbox Key";
    public GameObject droppedPrefab;

    public void Pickup()
    {
        PlayerInventory.Instance.AddItem(keyID, displayName, droppedPrefab);
        gameObject.SetActive(false);
    }
}