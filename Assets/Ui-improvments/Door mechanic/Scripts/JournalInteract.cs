using UnityEngine;

public class JournalInteract : MonoBehaviour
{
    public void Interact()
    {
        if (JournalUI.Instance != null)
            JournalUI.Instance.OpenJournal();
    }
}