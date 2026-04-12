using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactRange = 3f;
    public KeyCode interactKey = KeyCode.E;
    public KeyCode dropKey = KeyCode.Q;
    public Camera playerCamera;

    private KeyItem currentLookedAtItem = null;
    private LockboxUI lockboxUI;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        lockboxUI = FindObjectOfType<LockboxUI>();
    }

    void Update()
    {
        bool lockboxOpen = lockboxUI != null && lockboxUI.IsUIOpen();
        bool journalOpen = JournalUI.Instance != null && JournalUI.Instance.IsOpen();
        bool inventoryOpen = InventoryUI.IsInventoryOpen();
        bool tvOpen = TVObjectiveUI.Instance != null && TVObjectiveUI.Instance.IsOpen();

        if (lockboxOpen || journalOpen || inventoryOpen || tvOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            InteractionPromptUI.Instance.Hide();

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Tab))
            {
                if (lockboxOpen) lockboxUI.CloseUI();
                if (journalOpen) JournalUI.Instance.CloseJournal();
                if (inventoryOpen) InventoryUI.Instance.CloseInventory();
                if (tvOpen) TVObjectiveUI.Instance.ClosePanel();
            }
            return;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (Input.GetKeyDown(interactKey))
            TryInteract();

        if (Input.GetKeyDown(KeyCode.Tab))
            InventoryUI.Instance.OpenInventory();

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
        if (door != null) door.TryOpen();
    }

    void UpdatePrompt()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.TryGetComponent(out KeyItem key) && hit.collider.gameObject.activeSelf)
            {
                InteractionPromptUI.Instance.Show("[E] Ramasser");
                currentLookedAtItem = key;
                return;
            }

            if (hit.collider.TryGetComponent(out JournalPickup journal) && hit.collider.gameObject.activeSelf)
            {
                InteractionPromptUI.Instance.Show("[E] Ramasser le journal");
                currentLookedAtItem = null;
                return;
            }

            if (hit.collider.TryGetComponent(out LockboxUI lockbox))
            {
                if (!lockbox.IsUnlocked())
                    InteractionPromptUI.Instance.Show("[E] Ouvrir le coffre");
                else
                    InteractionPromptUI.Instance.Hide();
                currentLookedAtItem = null;
                return;
            }

            if (hit.collider.TryGetComponent(out Keylock keylock))
            {
                InteractionPromptUI.Instance.Show("[E] Utiliser la clé");
                currentLookedAtItem = null;
                return;
            }

            if (hit.collider.TryGetComponent(out JournalInteract journalInteract))
            {
                InteractionPromptUI.Instance.Show("[E] Lire le journal");
                currentLookedAtItem = null;
                return;
            }

            if (hit.collider.TryGetComponent(out TVInteract tv))
            {
                InteractionPromptUI.Instance.Show("[E] Regarder la télévision");
                currentLookedAtItem = null;
                return;
            }

            DoorController foundDoor = hit.collider.GetComponent<DoorController>();
            if (foundDoor == null) foundDoor = hit.collider.GetComponentInParent<DoorController>();
            if (foundDoor == null) foundDoor = hit.collider.GetComponentInChildren<DoorController>();
            if (foundDoor != null)
            {
                InteractionPromptUI.Instance.Show("[E] Ouvrir la porte");
                currentLookedAtItem = null;
                return;
            }
        }

        InteractionPromptUI.Instance.Hide();
        currentLookedAtItem = null;
    }

    void TryInteract()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            if (hit.collider.TryGetComponent(out LockboxUI lockbox))
                lockbox.OpenUI();
            else if (hit.collider.TryGetComponent(out KeyItem key))
                key.Pickup();
            else if (hit.collider.TryGetComponent(out JournalPickup journalPickup))
                journalPickup.Pickup();
            else if (hit.collider.TryGetComponent(out JournalInteract journalInteract))
                journalInteract.Interact();
            else if (hit.collider.TryGetComponent(out TVInteract tv))
                tv.Interact();
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