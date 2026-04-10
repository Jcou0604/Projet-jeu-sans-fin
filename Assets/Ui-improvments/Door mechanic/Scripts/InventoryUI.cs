using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    [Header("Inventory Panel")]
    public GameObject inventoryPanel;
    public Transform slotsParent;
    public GameObject slotPrefab;
    public int totalSlots = 8;

    [Header("Hotbar")]
    public Transform hotbarParent;
    public GameObject hotbarSlotPrefab;
    public int hotbarSlots = 4;
    public GameObject hotbarObject;

    private bool isOpen = false;
    private List<GameObject> slots = new List<GameObject>();
    private List<GameObject> hotbarSlotObjects = new List<GameObject>();

    public static bool IsInventoryOpen() => Instance != null && Instance.isOpen;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        inventoryPanel.SetActive(false);
        PlayerInventory.Instance.OnInventoryChanged += RefreshUI;
        CreateSlots();
        CreateHotbarSlots();
    }

    public void HideHotbar()
    {
        if (hotbarObject != null) hotbarObject.SetActive(false);
    }

    public void ShowHotbar()
    {
        if (hotbarObject != null) hotbarObject.SetActive(true);
    }

    public void OpenInventory()
    {
        isOpen = true;
        inventoryPanel.SetActive(true);
        HideHotbar();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        RefreshUI();
    }

    public void CloseInventory()
    {
        isOpen = false;
        inventoryPanel.SetActive(false);
        ShowHotbar();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void CreateSlots()
    {
        for (int i = 0; i < totalSlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            slot.name = "Slot_" + i;
            slots.Add(slot);
            SetSlotEmpty(slot);
        }
    }

    void CreateHotbarSlots()
    {
        if (hotbarParent == null || hotbarSlotPrefab == null) return;
        for (int i = 0; i < hotbarSlots; i++)
        {
            GameObject slot = Instantiate(hotbarSlotPrefab, hotbarParent);
            slot.name = "HotbarSlot_" + i;
            hotbarSlotObjects.Add(slot);
            SetSlotEmpty(slot);
        }
    }

    void RefreshUI()
    {
        Dictionary<string, string> items = PlayerInventory.Instance.GetAllItems();
        List<string> itemIDs = new List<string>(items.Keys);
        List<string> itemNames = new List<string>(items.Values);

        for (int i = 0; i < slots.Count; i++)
        {
            if (i < itemNames.Count)
            {
                SetSlotFilled(slots[i], itemNames[i]);
                string capturedID = itemIDs[i];
                Button btn = slots[i].GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.RemoveAllListeners();
                    btn.onClick.AddListener(() =>
                    {
                        PlayerInventory.Instance.UseItem(capturedID);
                        CloseInventory();
                    });
                }
            }
            else
            {
                SetSlotEmpty(slots[i]);
                Button btn = slots[i].GetComponent<Button>();
                if (btn != null) btn.onClick.RemoveAllListeners();
            }
        }

        for (int i = 0; i < hotbarSlotObjects.Count; i++)
        {
            if (i < itemNames.Count)
                SetSlotFilled(hotbarSlotObjects[i], itemNames[i]);
            else
                SetSlotEmpty(hotbarSlotObjects[i]);
        }
    }

    void SetSlotFilled(GameObject slot, string itemName)
    {
        Image border = slot.GetComponent<Image>();
        if (border != null)
            border.color = new Color(0.78f, 0.66f, 0.29f, 1f);

        TextMeshProUGUI label = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
        {
            label.text = itemName;
            label.fontSize = 11;
            label.color = new Color(0.91f, 0.87f, 0.78f, 1f);
            label.alignment = TextAlignmentOptions.Center;
        }
    }

    void SetSlotEmpty(GameObject slot)
    {
        Image border = slot.GetComponent<Image>();
        if (border != null)
            border.color = new Color(0.15f, 0.10f, 0.07f, 1f);

        TextMeshProUGUI label = slot.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null) label.text = "";
    }

    void OnDestroy()
    {
        if (PlayerInventory.Instance != null)
            PlayerInventory.Instance.OnInventoryChanged -= RefreshUI;
    }
}