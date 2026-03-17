// PlayerInteraction.cs
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public Camera playerCamera;

    void Update()
    {
        if (Input.GetKeyDown(interactKey))
            TryInteract();
    }

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position,
                          playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            // Check for each interactable type
            if (hit.collider.TryGetComponent(out LockboxUI lockbox))
                lockbox.OpenUI();

            else if (hit.collider.TryGetComponent(out KeyItem key))
                key.Pickup();

            else if (hit.collider.TryGetComponent(out DoorController door))
                door.TryOpen();
        }
    }
}