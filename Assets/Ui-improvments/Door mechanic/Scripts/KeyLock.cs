using UnityEngine;

public class Keylock : MonoBehaviour
{
    public string requiredKeyID = "LockboxKey";
    public DoorController linkedDoor;
    private bool isUnlocked = false;

    public void TryUnlock()
    {
        Debug.Log("TryUnlock called!");

        if (isUnlocked)
        {
            Debug.Log("Already unlocked!");
            return;
        }

        if (PlayerInventory.Instance.HasItem(requiredKeyID))
        {
            Debug.Log("Has key, unlocking door!");
            isUnlocked = true;
            PlayerInventory.Instance.RemoveItem(requiredKeyID);
            linkedDoor.UnlockDoor();
        }
        else
        {
            Debug.Log("Key not found! Looking for: " + requiredKeyID);
            Debug.Log("Items in inventory: " +
                string.Join(", ", PlayerInventory.Instance.GetAllItems().Keys));
        }
    }
}