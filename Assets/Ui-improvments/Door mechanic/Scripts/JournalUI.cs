using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JournalUI : MonoBehaviour
{
    public static JournalUI Instance;

    [Header("UI References")]
    public GameObject journalPanel;
    public TextMeshProUGUI pageText;
    public TextMeshProUGUI pageNumberText;
    public Button nextButton;
    public Button prevButton;

    private int currentPage = 0;

    private string[] pages = new string[]
    {
        "Jour 1\n\nJe ne sais pas qui trouvera ceci.\nPeut-être personne.\nPeut-être Sami, quand il sera assez grand pour comprendre.\n\nNous sommes en guerre.\nEt je veux que quelqu'un sache que nous étions là.",

        "Jour 1 — suite\n\nLes bombes ont recommencé à l'aube.\nJe l'ai tenu sous la table jusqu'à ce que tout s'arrête.\n\nIl n'a pas pleuré.\nIl est plus courageux que je ne l'ai jamais été.",

        "Jour 4\n\nMaryam nous a quittés la semaine dernière.\nIl n'y a pas eu d'avertissement.\n\nSon anniversaire était le 7 octobre.\nJe n'oublierai jamais cette date.\n\nMaryam, tu me manques.",

        "Jour 4 — suite\n\nJ'ai mis son anniversaire comme code du coffre.\n\n0710\n\nC'est là que je garde nos souvenirs —\nnos photos, ses lettres.\nTant que le coffre existe, elle existe.",

        "Avant l'aube\n\nSami — tu es tout mon monde.\nSois courageux. Sois gentil. Survie.\n\nTon Baba"
    };

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        journalPanel.SetActive(false);
    }

    public bool IsOpen()
    {
        return journalPanel.activeSelf;
    }

    public void OpenJournal()
    {
        currentPage = 0;
        journalPanel.SetActive(true);
        UpdatePage();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(HideHotbarNextFrame());
    }

    private System.Collections.IEnumerator HideHotbarNextFrame()
    {
        yield return null;
        if (InventoryUI.Instance != null) InventoryUI.Instance.HideHotbar();
    }

    public void CloseJournal()
    {
        journalPanel.SetActive(false);
        if (InventoryUI.Instance != null) InventoryUI.Instance.ShowHotbar();
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
        pageText.text = pages[currentPage];
        pageNumberText.text = (currentPage + 1) + " / " + pages.Length;
        prevButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage < pages.Length - 1;
    }

    void Update()
    {
        if (journalPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseJournal();
    }
}