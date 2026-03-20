using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public Camera playerCamera;

    private KeyItem currentLookedAtItem = null;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Close lockbox with Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockboxUI openBox = FindObjectOfType<LockboxUI>();
            if (openBox != null) openBox.CloseUI();
        }

        // Handle E key
        if (Input.GetKeyDown(interactKey))
        {
            LockboxUI openBox = FindObjectOfType<LockboxUI>();
            if (openBox != null && openBox.IsUIOpen())
                openBox.CloseUI();
            else
                TryInteract();
        }

        // Handle Q key — drop the first item in inventory
        if (Input.GetKeyDown(dropKey))
        {
            var items = PlayerInventory.Instance.GetAllItems();
            foreach (var item in items)
            {
                DropSystem.Instance.DropItem(item.Key);
                break; // Drop only first item
            }
        }

        // Update the interaction prompt every frame
        UpdatePrompt();
    }

    void UpdatePrompt()
    {
        Ray ray = new Ray(playerCamera.transform.position,
                          playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
            // Looking at a key on the floor
            if (hit.collider.TryGetComponent(out KeyItem key)
                && hit.collider.gameObject.activeSelf)
            {
                InteractionPromptUI.Instance.Show(
                    "[E] Pick up   [Q] Leave");
                currentLookedAtItem = key;
                return;
            }

            // Looking at the lockbox
            if (hit.collider.TryGetComponent(out LockboxUI lockbox))
            {
                InteractionPromptUI.Instance.Show("[E] Open lockbox");
                currentLookedAtItem = null;
                return;
            }

            // Looking at the keylock
            if (hit.collider.TryGetComponent(out Keylock keylock))
            {
                InteractionPromptUI.Instance.Show("[E] Use key");
                currentLookedAtItem = null;
                return;
            }

            // Looking at the door
            if (hit.collider.TryGetComponent(out DoorController door))
            {
                InteractionPromptUI.Instance.Show("[E] Open door");
                currentLookedAtItem = null;
                return;
            }
        }

        // Not looking at anything interactable
        InteractionPromptUI.Instance.Hide();
        currentLookedAtItem = null;
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

            else if (hit.collider.TryGetComponent(out Keylock keylock))
                keylock.TryUnlock();

            else if (hit.collider.TryGetComponent(out DoorController door))
                door.TryOpen();
        }
    }
}