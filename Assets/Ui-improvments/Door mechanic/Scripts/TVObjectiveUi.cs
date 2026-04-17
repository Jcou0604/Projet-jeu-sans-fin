using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVObjectiveUI : MonoBehaviour
{
    public static TVObjectiveUI Instance;

    [Header("UI References")]
    public GameObject objectivePanel;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        objectivePanel.SetActive(false);
    }

    public bool IsOpen()
    {
        return objectivePanel.activeSelf;
    }

    public void OpenPanel()
    {
        objectivePanel.SetActive(true);
        StartCoroutine(HideHotbarNextFrame());
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePanel()
    {
        objectivePanel.SetActive(false);
        if (InventoryUI.Instance != null) InventoryUI.Instance.ShowHotbar();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private System.Collections.IEnumerator HideHotbarNextFrame()
    {
        yield return null;
        if (InventoryUI.Instance != null) InventoryUI.Instance.HideHotbar();
    }

    void Update()
    {
        if (objectivePanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            ClosePanel();
    }
}