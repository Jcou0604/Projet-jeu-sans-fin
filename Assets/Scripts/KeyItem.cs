using UnityEngine;

public class KeyItem : MonoBehaviour
{
    public string keyID = "LockboxKey";
    public string displayName = "Lockbox Key";

    public void Pickup()
    {
        PlayerInventory.Instance.AddItem(keyID, displayName);
        gameObject.SetActive(false); // Hide it, don't destroy
    }
}