using UnityEngine;

public class Keylock : MonoBehaviour
{
    public string requiredKeyID = "LockboxKey";
    public DoorController linkedDoor;
    private bool isUnlocked = false;

    public void TryUnlock()
    {
        if (isUnlocked) return;

        if (PlayerInventory.Instance.HasItem(requiredKeyID))
        {
            isUnlocked = true;
            PlayerInventory.Instance.RemoveItem(requiredKeyID);
            linkedDoor.UnlockDoor();
            linkedDoor.TryOpen();
        }
        else
        {
            Debug.Log("Clé introuvable. Cherche: " + requiredKeyID);
        }
    }
}