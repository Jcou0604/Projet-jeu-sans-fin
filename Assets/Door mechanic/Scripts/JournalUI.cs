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
        "Day 1\n\nI do not know who will find this. Maybe no one. Maybe Sami, when he is old enough to understand.\n\nThe bombs started again at dawn. I held him under the table until the shaking stopped. He did not cry. He is braver than I ever was.\n\nI am writing this so that someone knows we were here. That we lived. That we loved.",

        "Day 4\n\nMaryam used to say that fear is just love with nowhere to go.\n\nI think about her every hour. She was taken from us in the second week of the strikes. There was no warning. There never is.\n\nHer birthday was the seventh of October. I always forgot to buy flowers early enough. She would laugh and say the date was impossible to forget anyway.\n\n— 0710 —\n\nNow I understand why.",

        "Day 9\n\nSami asked me today if Mama was in heaven or just gone.\n\nI told him heaven. I hope I was not lying.\n\nحبيبتي. My love. I carry you with me into whatever comes next.",

        "Day 14\n\nI have been asked to join the others at the front. I said I needed one more day with Sami.\n\nThey gave me until morning.\n\nI hid what I could in the basement. Food. Water. The old chest — Sami knows how we used to lock it when he was small, the little game we played. He will remember.\n\nI pray he finds this before he finds me.",

        "Day 15 — Before dawn\n\nI am not a soldier. I am a teacher. I taught mathematics and history to children who deserved a future.\n\nBut there is no one else left on this street.\n\nIf you are reading this and you are not my son — please. Get him out. He is seven years old and he is alone in the basement and he is the best thing I ever did.\n\nإذا وجدته، أرجوك اعتني به.\nIf you find him, please take care of him.",

        "The last page is blank except for a small drawing in the corner — a man and a boy, stick figures, holding hands under a sun.\n\nBelow it, in careful handwriting:\n\n'Sami — you are my whole world. Be brave. Be kind. Survive.\n\nAll my love,\nYour Baba'"
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
    }

    public void CloseJournal()
    {
        journalPanel.SetActive(false);
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