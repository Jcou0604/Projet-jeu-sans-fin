using UnityEngine;

public class Keylock : MonoBehaviour
{
    public string requiredKeyID = "LockboxKey";
    public DoorController linkedDoor;
    private bool isUnlocked = false;
    public GameObject Door;

    public void TryUnlock()
    {
        if (isUnlocked) return;

        if (PlayerInventory.Instance.HasItem(requiredKeyID))
        {
            isUnlocked = true;
            PlayerInventory.Instance.RemoveItem(requiredKeyID);
            linkedDoor.UnlockDoor();
            linkedDoor.TryOpen();
            Door.GetComponent<Collider>().enabled = false;
        }
        else
        {
            Debug.Log("Clé introuvable. Cherche: " + requiredKeyID);
        }
    }
}