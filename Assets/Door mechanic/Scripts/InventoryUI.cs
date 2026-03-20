using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform slotsParent;
    public GameObject slotPrefab;
    public int totalSlots = 8;

    private bool isOpen = false;
    private List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        inventoryPanel.SetActive(false);
        PlayerInventory.Instance.OnInventoryChanged += RefreshUI;
        CreateSlots();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            inventoryPanel.SetActive(isOpen);

            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;

            if (isOpen) RefreshUI();
        }
    }

    void CreateSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            slot.name = $"Slot_{i}";
            slots.Add(slot);
            // Clear slot to empty state
            SetSlotEmpty(slot);
        }
    }

    void RefreshUI()
    {
        Dictionary<string, string> items =
            PlayerInventory.Instance.GetAllItems();
        List<string> itemNames = new List<string>(items.Values);

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < itemNames.Count)
                SetSlotFilled(slots[i], itemNames[i]);
            else
                SetSlotEmpty(slots[i]);
        }
    }

    void SetSlotFilled(GameObject slot, string itemName)
    {
        // Highlight border
        Image border = slot.GetComponent<Image>();
        if (border != null)
            border.color = new Color(0.78f, 0.66f, 0.29f, 1f); // Gold

        // Set item name text
        TextMeshProUGUI label = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
            label.text = itemName;
    }

    void SetSlotEmpty(GameObject slot)
    {
        Image border = slot.GetComponent<Image>();
        if (border != null)
            border.color = new Color(0.25f, 0.25f, 0.3f, 1f); // Dark grey

        TextMeshProUGUI label = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
            label.text = "";
    }

    void OnDestroy()
    {
        if (PlayerInventory.Instance != null)
            PlayerInventory.Instance.OnInventoryChanged -= RefreshUI;
    }
}