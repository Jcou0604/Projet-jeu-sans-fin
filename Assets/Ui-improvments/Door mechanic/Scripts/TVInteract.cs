using UnityEngine;

public class TVInteract : MonoBehaviour
{
    public void Interact()
    {
        if (TVObjectiveUI.Instance != null)
            TVObjectiveUI.Instance.OpenPanel();
    }
}