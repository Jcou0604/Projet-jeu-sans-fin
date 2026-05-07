using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TVObjectiveUI : MonoBehaviour
{
    public static TVObjectiveUI Instance;

    [Header("UI References")]
    public GameObject objectivePanel;
    public TextMeshProUGUI pageText;
    public TextMeshProUGUI pageNumberText;
    public Button nextButton;
    public Button prevButton;

    private int currentPage = 0;

    private string[] pages = new string[]
    {
        "OBJECTIFS\n\n□  Trouve le journal de ton père.\n\n□  Lis le journal pour trouver le code du coffre.\n\n□  Ouvre le coffre et récupère la clé.\n\n□  Utilise la clé pour ouvrir la porte et t'échapper.\n\n\nTu n'as pas beaucoup de temps.\nBonne chance.",

        "COMMANDES\n\n[W A S D]  —  Se déplacer\n\n[Souris]   —  Regarder autour\n\n[E]  —  Interagir / Ramasser un objet\n\n[Tab]  —  Ouvrir l'inventaire\n           (clique sur un objet pour l'utiliser)\n\n[Échap]  —  Fermer un panneau"
    };

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
        currentPage = 0;
        objectivePanel.SetActive(true);
        UpdatePage();
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

    public void NextPage()
    {
        if (currentPage < pages.Length - 1)
        {
            currentPage++;
            UpdatePage();
        }
    }

    public void PrevPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }

    void UpdatePage()
    {
        if (pageText != null) pageText.text = pages[currentPage];
        if (pageNumberText != null) pageNumberText.text = (currentPage + 1) + " / " + pages.Length;
        if (prevButton != null) prevButton.interactable = currentPage > 0;
        if (nextButton != null) nextButton.interactable = currentPage < pages.Length - 1;
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