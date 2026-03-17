using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public Camera playerCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockboxUI openBox = FindObjectOfType<LockboxUI>();
            if (openBox != null) openBox.CloseUI();
        }

        if (Input.GetKeyDown(interactKey))
            TryInteract();
    }

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position,
                          playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.TryGetComponent(out LockboxUI lockbox))
                lockbox.OpenUI();

            else if (hit.collider.TryGetComponent(out KeyItem key))
                key.Pickup();

            else if (hit.collider.TryGetComponent(out DoorController door))
                door.TryOpen();
        }
    }
}