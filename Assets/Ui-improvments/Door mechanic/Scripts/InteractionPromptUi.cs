using UnityEngine;
using TMPro;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance;

    public GameObject promptPanel;
    public TextMeshProUGUI promptText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Hide();
    }

    public void Show(string message)
    {
        promptPanel.SetActive(true);
        promptText.text = message;
    }

    public void Hide()
    {
        promptPanel.SetActive(false);
    }
}