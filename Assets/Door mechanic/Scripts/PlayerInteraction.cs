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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LockboxUI openBox = FindObjectOfType<LockboxUI>();
            if (openBox != null) openBox.CloseUI();
        }

        if (Input.GetKeyDown(interactKey))
        {
            LockboxUI openBox = FindObjectOfType<LockboxUI>();
            if (openBox != null && openBox.IsUIOpen())
                openBox.CloseUI();
            else
                TryInteract();
        }

        if (Input.GetKeyDown(dropKey))
        {
            var items = PlayerInventory.Instance.GetAllItems();
            foreach (var item in items)
            {
                DropSystem.Instance.DropItem(item.Key);
                break;
            }
        }

        UpdatePrompt();
    }

    void OnTriggerEnter(Collider other)
    {
        DoorController door = other.GetComponent<DoorController>();
        if (door == null) door = other.GetComponentInParent<DoorController>();
        if (door == null) door = other.GetComponentInChildren<DoorController>();
        if (door != null)
            door.TryOpen();
    }

    void UpdatePrompt()
    {
        Ray ray = new Ray(playerCamera.transform.position,
                          playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            // Looking at a key on the floor
            if (hit.collider.TryGetComponent(out KeyItem key)
                && hit.collider.gameObject.activeSelf)
            {
                InteractionPromptUI.Instance.Show("[E] Pick up");
                currentLookedAtItem = key;
                return;
            }

            // Looking at the lockbox — only show prompt if not yet unlocked
            if (hit.collider.TryGetComponent(out LockboxUI lockbox))
            {
                if (!lockbox.IsUnlocked())
                {
                    InteractionPromptUI.Instance.Show("[E] Open chest");
                    currentLookedAtItem = null;
                    return;
                }
                // If unlocked, don't show any prompt for the chest
                InteractionPromptUI.Instance.Hide();
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
            DoorController foundDoor = hit.collider.GetComponent<DoorController>();
            if (foundDoor == null) foundDoor = hit.collider.GetComponentInParent<DoorController>();
            if (foundDoor == null) foundDoor = hit.collider.GetComponentInChildren<DoorController>();
            if (foundDoor != null)
            {
                InteractionPromptUI.Instance.Show("[E] Open door");
                currentLookedAtItem = null;
                return;
            }
        }

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

            else
            {
                DoorController door = hit.collider.GetComponent<DoorController>();
                if (door == null) door = hit.collider.GetComponentInParent<DoorController>();
                if (door == null) door = hit.collider.GetComponentInChildren<DoorController>();
                if (door != null) door.TryOpen();
            }
        }
    }
}