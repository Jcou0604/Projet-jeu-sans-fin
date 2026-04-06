using UnityEngine;

public class JournalPickup : MonoBehaviour
{
    public string itemID = "Journal";
    public string displayName = "Father's Journal";

    public void Pickup()
    {
        PlayerInventory.Instance.AddItem(itemID, displayName, null, () =>
        {
            if (JournalUI.Instance != null)
                JournalUI.Instance.OpenJournal();
        });
        gameObject.SetActive(false);
    }
}